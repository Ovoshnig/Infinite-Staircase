using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using VContainer;

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
    public void Construct(IMusicLoader musicLoader, MusicQueue musicQueue,
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
    }

    private void Start()
    {
        _sceneSwitch.IsSceneLoading
            .Where(value => !value)
            .Subscribe(value => TryPlayMusic())
            .AddTo(this);
    }

    private void OnDestroy() => _cts.CancelAndDispose(ref _cts);

    private bool TryPlayMusic()
    {
        _cts.CancelAndDispose(ref _cts);
        _cts = new CancellationTokenSource();

        MusicCategory category = _sceneMusicMapper.GetMusicCategory(_sceneSwitch.CurrentSceneType);

        if (_musicClipPaths.TryGetValue(category, out List<string> clips))
        {
            PlayMusic(clips);

            return true;
        }
        else
        {
            Debug.LogWarning($"No music found for category {category}");

            return false;
        }
    }

    private void PlayMusic(List<string> clipPaths)
    {
        _musicQueue.SetClips(clipPaths);
        _musicQueue.Shuffle();
        PlayNextClipAsync().Forget();
    }

    private async UniTask PlayNextClipAsync()
    {
        if (_pastClip != null)
        {
            ReleaseClip(_pastClip);
            _pastClip = null;
        }

        string nextClipPath = _musicQueue.GetNextClip();

        if (nextClipPath == null)
            return;

        AudioClip clip = await _musicLoader.LoadClipAsync(nextClipPath, _cts.Token);
        _audioSource.clip = clip;
        _pastClip = clip;
        _audioSource.Play();
        await UniTask.WaitWhile(() => _audioSource.isPlaying, cancellationToken: _cts.Token);

        PlayNextClipAsync().Forget();
    }

    private void ReleaseClip(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = null;
        Resources.UnloadAsset(clip);
    }

    private Dictionary<MusicCategory, List<string>> LoadMusicClipPaths()
    {
        Dictionary<MusicCategory, List<string>> musicClipKeys = new();
        TextAsset resourcesList = Resources.Load<TextAsset>(ResourcesConstants.ResourcesListName);

        if (resourcesList == null)
        {
            Debug.LogError($"Ќе удалось загрузить список ресурсов.");

            return musicClipKeys;
        }

        char[] separators = { '\r', '\n' };
        string extension = ".mp3";

        foreach (MusicCategory category in Enum.GetValues(typeof(MusicCategory)))
        {
            string categoryPath = $"{ResourcesConstants.MusicPath}/{category}";

            List<string> clipPaths = new(resourcesList.text
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Where(c => c.StartsWith(categoryPath) && c.EndsWith(extension))
                .Select(c => c[..^4]));

            musicClipKeys[category] = clipPaths;
        }

        return musicClipKeys;
    }
}
