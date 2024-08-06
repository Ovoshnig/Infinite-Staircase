using Cysharp.Threading.Tasks;
using Random = System.Random;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    private enum MusicCategory
    {
        MainMenu,
        GameLevel,
        Credits
    }

    private readonly Dictionary<MusicCategory, List<string>> _musicTrackKeys = new();
    private readonly Random _random = new();
    private Queue<string> _trackKeysQueue = new();
    private AudioSource _audioSource;
    private SceneSwitch _sceneSwitch;
    private AsyncOperationHandle<AudioClip>? _currentTrackHandle = null;
    private CancellationTokenSource _cts = default;

    [Inject]
    private void Construct(SceneSwitch sceneSwitch) => _sceneSwitch = sceneSwitch;

    private void Awake() => _audioSource = gameObject.GetComponent<AudioSource>();

    private async void Start()
    {
        _cts = new CancellationTokenSource();
        await LoadMusicTrackKeys();
        _sceneSwitch.SceneLoaded += OnSceneLoaded;
        OnSceneLoaded(_sceneSwitch.CurrentSceneType);
    }

    private void OnDestroy()
    {
        _sceneSwitch.SceneLoaded -= OnSceneLoaded;
        CancelToken();
    }

    private async UniTask LoadMusicTrackKeys()
    {
        foreach (MusicCategory category in Enum.GetValues(typeof(MusicCategory)))
        {
            string label = category.ToString();
            var handle = Addressables.LoadResourceLocationsAsync(label, typeof(AudioClip));
            await handle.ToUniTask(cancellationToken: _cts.Token);

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                List<string> keys = new();

                foreach (var location in handle.Result)
                    keys.Add(location.PrimaryKey);

                _musicTrackKeys[category] = keys;
            }
            else
            {
                Debug.LogWarning($"Failed to load music track keys for label: {label}");
                _musicTrackKeys[category] = new List<string>();
            }

            Addressables.Release(handle);
        }
    }

    private void OnSceneLoaded(SceneSwitch.SceneType scene)
    {
        CancelToken();
        _cts = new CancellationTokenSource();

        Dictionary<SceneSwitch.SceneType, MusicCategory> sceneToMusicCategory = new()
        {
            { SceneSwitch.SceneType.MainMenu, MusicCategory.MainMenu },
            { SceneSwitch.SceneType.GameLevel, MusicCategory.GameLevel },
            { SceneSwitch.SceneType.Credits, MusicCategory.Credits }
        };

        MusicCategory category;

        if (sceneToMusicCategory.ContainsKey(scene))
            category = sceneToMusicCategory[scene];
        else
            category = MusicCategory.MainMenu;

        PlayNextTrack(category).Forget();
    }

    private bool TrySetCurrentTracks(MusicCategory category)
    {
        if (!_musicTrackKeys.ContainsKey(category))
        {
            Debug.LogError($"Music category '{category}' not found in _musicTrackKeys dictionary.");

            return false;
        }

        if (_musicTrackKeys[category].Count == 0)
        {
            Debug.LogWarning($"There are no tracks for category {category}");

            return false;
        }

        _trackKeysQueue.Clear();
        _trackKeysQueue = new Queue<string>(_musicTrackKeys[category]);

        return true;
    }

    private void ShuffleAndQueueTracks()
    {
        int tracksCount = _trackKeysQueue.Count;
        List<string> temporaryList = new(_trackKeysQueue);
        _trackKeysQueue.Clear();

        for (int i = 0; i < tracksCount; i++)
        {
            int index = _random.Next(temporaryList.Count);
            _trackKeysQueue.Enqueue(temporaryList[index]);
            temporaryList.RemoveAt(index);
        }
    }

    private async UniTask<AsyncOperationHandle<AudioClip>> LoadTrack(string key)
    {
        var handle = Addressables.LoadAssetAsync<AudioClip>(key);
        await handle.ToUniTask(cancellationToken: _cts.Token);

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle;
        }
        else
        {
            Debug.LogError($"Failed to load track with key: {key}");

            return default;
        }
    }

    private void ReleaseTrack(AsyncOperationHandle<AudioClip> handle)
    {
        if (handle.IsValid())
            Addressables.Release(handle);
    }

    private async UniTask PlayNextTrack(MusicCategory category)
    {
        if (_trackKeysQueue.Count == 0)
        {
            if (TrySetCurrentTracks(category))
                ShuffleAndQueueTracks();
            else
                return;
        }

        if (_currentTrackHandle.HasValue)
        {
            _audioSource.Stop();
            _audioSource.clip = null;
            ReleaseTrack(_currentTrackHandle.Value);
            _currentTrackHandle = null;
        }

        string trackKey = _trackKeysQueue.Dequeue();
        var trackHandle = await LoadTrack(trackKey);

        if (trackHandle.Status == AsyncOperationStatus.Succeeded)
        {
            _currentTrackHandle = trackHandle;
            _audioSource.clip = trackHandle.Result;
            _audioSource.Play();
            await UniTask.WaitWhile(() => _audioSource.isPlaying, cancellationToken: _cts.Token);

            if (_cts.Token.IsCancellationRequested)
            {
                ReleaseTrack(trackHandle);
                return;
            }

            PlayNextTrack(category).Forget();
        }
    }

    private void CancelToken()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        if (_currentTrackHandle.HasValue)
        {
            ReleaseTrack(_currentTrackHandle.Value);
            _currentTrackHandle = null;
        }
    }
}
