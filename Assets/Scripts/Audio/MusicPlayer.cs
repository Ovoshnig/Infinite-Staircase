using Cysharp.Threading.Tasks;
using Random = System.Random;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
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

    private readonly Dictionary<MusicCategory, List<string>> _musicClipKeys = new();
    private readonly Random _random = new();
    private Queue<string> _clipPathsQueue = new();
    private AudioClip _pastClip = null;
    private AudioSource _audioSource;
    private SceneSwitch _sceneSwitch;
    private CancellationTokenSource _cts = default;

    [Inject]
    private void Construct(SceneSwitch sceneSwitch) => _sceneSwitch = sceneSwitch;

    private void Awake()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();

        _cts = new CancellationTokenSource();

        LoadMusicClipNames();
        _sceneSwitch.SceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        _sceneSwitch.SceneLoaded -= OnSceneLoaded;
        _cts.CancelAndDispose(ref _cts);
    }

    private void LoadMusicClipNames()
    {
        foreach (MusicCategory category in Enum.GetValues(typeof(MusicCategory)))
        {
            var categoryPath = $"{ResourcesConstants.ResourcesPath}/{ResourcesConstants.MusicPath}/{category}";
            var extension = ".mp3";

            if (!Directory.Exists(categoryPath))
                throw new DirectoryNotFoundException(categoryPath);

            List<string> clipNames = Directory.GetFiles(categoryPath).ToList();
            clipNames = clipNames
                .Where(x => x.EndsWith(extension))
                .Select(x => x[(ResourcesConstants.ResourcesPath.Length + 1)..^extension.Length])
                .ToList();

            _musicClipKeys[category] = clipNames;
        }
    }

    private void OnSceneLoaded(SceneSwitch.SceneType scene)
    {
        _cts.CancelAndDispose(ref _cts);
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

        _clipPathsQueue.Clear();
        PlayNextClip(category).Forget();
    }

    private bool TrySetCurrentClips(MusicCategory category)
    {
        if (!_musicClipKeys.ContainsKey(category))
        {
            Debug.LogError($"Music category '{category}' not found in {nameof(_musicClipKeys)} dictionary.");

            return false;
        }

        if (_musicClipKeys[category].Count == 0)
        {
            Debug.LogWarning($"There are no clips for category {category}");

            return false;
        }

        _clipPathsQueue = new Queue<string>(_musicClipKeys[category]);

        return true;
    }

    private void ShuffleAndQueueClips()
    {
        int clipsCount = _clipPathsQueue.Count;
        List<string> temporaryList = new(_clipPathsQueue);
        _clipPathsQueue.Clear();

        for (int i = 0; i < clipsCount; i++)
        {
            int index = _random.Next(temporaryList.Count);
            _clipPathsQueue.Enqueue(temporaryList[index]);
            temporaryList.RemoveAt(index);
        }
    }

    private async UniTask<AudioClip> LoadClip(string path)
    {
        var request = Resources.LoadAsync<AudioClip>(path);
        var clip = await request.ToUniTask(cancellationToken: _cts.Token) as AudioClip;

        return clip;
    }

    private void ReleaseClip(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = null;
        Resources.UnloadAsset(clip);
    }

    private async UniTask PlayNextClip(MusicCategory category)
    {
        if (_clipPathsQueue.Count == 0)
        {
            if (TrySetCurrentClips(category))
                ShuffleAndQueueClips();
            else
                return;
        }

        if (_pastClip != null)
        {
            ReleaseClip(_pastClip);
            _pastClip = null;
        }

        var clipPath = _clipPathsQueue.Dequeue();
        var clip = await LoadClip(clipPath);
        _pastClip = clip;
        _audioSource.clip = clip;
        _audioSource.Play();
        await UniTask.WaitWhile(() => _audioSource.isPlaying, cancellationToken: _cts.Token);

        PlayNextClip(category).Forget();
    }
}
