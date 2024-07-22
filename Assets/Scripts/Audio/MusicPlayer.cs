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
    private Queue<string> _tracksQueue = new();
    private AudioSource _audioSource;
    private SceneSwitch _sceneSwitch;
    private CancellationTokenSource _cts = default;

    [Inject]
    private void Construct(SceneSwitch sceneSwitch) => _sceneSwitch = sceneSwitch;

    private void Awake() => _audioSource = gameObject.GetComponent<AudioSource>();

    private async void Start()
    {
        await LoadMusicTrackKeys();
        _sceneSwitch.SceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneSwitch.SceneType.GameLevel);
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
            await handle.Task;

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

        _tracksQueue.Clear();
        _tracksQueue = new Queue<string>(_musicTrackKeys[category]);

        return true;
    }

    private void ShuffleAndQueueTracks()
    {
        int tracksCount = _tracksQueue.Count;
        List<string> temporaryList = new(_tracksQueue);
        _tracksQueue.Clear();

        for (int i = 0; i < tracksCount; i++)
        {
            int index = _random.Next(temporaryList.Count);
            _tracksQueue.Enqueue(temporaryList[index]);
            temporaryList.RemoveAt(index);
        }
    }

    private async UniTask<AudioClip> LoadTrackByKey(string key)
    {
        var handle = Addressables.LoadAssetAsync<AudioClip>(key);
        await handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            AudioClip track = handle.Result;
            Addressables.Release(handle);

            return track;
        }
        else
        {
            Debug.LogError($"Failed to load track with key: {key}");

            return null;
        }
    }

    private async UniTask PlayNextTrack(MusicCategory category)
    {
        if (_tracksQueue.Count == 0)
        {
            if (TrySetCurrentTracks(category))
                ShuffleAndQueueTracks();
            else
                return;
        }

        AudioClip track = await LoadTrackByKey(_tracksQueue.Dequeue());
        _audioSource.clip = track;
        _audioSource.Play();
        await UniTask.WaitWhile(() => _audioSource.isPlaying, cancellationToken: _cts.Token);

        PlayNextTrack(category).Forget();
    }

    private void CancelToken()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }
}
