using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using Zenject;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    private MusicQueue _musicQueue;
    private IMusicLoader _musicLoader;
    private ISceneMusicMapper _sceneMusicMapper;
    private SceneSwitch _sceneSwitch;
    private Dictionary<MusicCategory, List<string>> _musicClipPaths;
    private CancellationTokenSource _cts = new();
    private AudioClip _pastClip = null;

    [Inject]
    private void Construct(IMusicLoader musicLoader, MusicQueue musicQueue,
        ISceneMusicMapper sceneMusicMapper, SceneSwitch sceneSwitch)
    {
        _musicLoader = musicLoader;
        _musicQueue = musicQueue;
        _sceneMusicMapper = sceneMusicMapper;
        _sceneSwitch = sceneSwitch;
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _musicClipPaths = LoadMusicClipPaths();

        _sceneSwitch.SceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        _cts.CancelAndDispose(ref _cts);

        _sceneSwitch.SceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(SceneSwitch.SceneType sceneType)
    {
        _cts.CancelAndDispose(ref _cts);
        _cts = new CancellationTokenSource();

        var category = _sceneMusicMapper.GetMusicCategory(sceneType);

        if (_musicClipPaths.TryGetValue(category, out var clips))
            PlayMusic(clips);
        else
            Debug.LogWarning($"No music found for category {category}");
    }

    private void PlayMusic(List<string> clipPaths)
    {
        _musicQueue.SetClips(clipPaths);
        _musicQueue.Shuffle();
        PlayNextClip().Forget();
    }

    private async UniTask PlayNextClip()
    {
        if (_pastClip != null)
        {
            ReleaseClip(_pastClip);
            _pastClip = null;
        }

        var nextClipPath = _musicQueue.GetNextClip();

        if (nextClipPath == null)
            return;

        var clip = await _musicLoader.LoadClipAsync(nextClipPath, _cts.Token);
        _audioSource.clip = clip;
        _pastClip = clip;
        _audioSource.Play();
        await UniTask.WaitWhile(() => _audioSource.isPlaying, cancellationToken: _cts.Token);

        PlayNextClip().Forget();
    }

    private void ReleaseClip(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = null;
        Resources.UnloadAsset(clip);
    }

    private Dictionary<MusicCategory, List<string>> LoadMusicClipPaths()
    {
        var musicClipKeys = new Dictionary<MusicCategory, List<string>>();

        foreach (MusicCategory category in Enum.GetValues(typeof(MusicCategory)))
        {
            var categoryPath = $"{ResourcesConstants.MusicPath}/{category}/clipList";

            TextAsset clipListAsset = Resources.Load<TextAsset>(categoryPath);

            if (clipListAsset == null)
            {
                Debug.LogError($"Ќе удалось загрузить список клипов дл€ категории {category}.");
                continue;
            }

            List<string> clipNames = new(clipListAsset.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            musicClipKeys[category] = clipNames;
        }

        return musicClipKeys;
    }
}
