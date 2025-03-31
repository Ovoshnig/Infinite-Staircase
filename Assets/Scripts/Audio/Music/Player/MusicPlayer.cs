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
    private IClipLoader _clipLoader;
    private ISceneMusicMapper _sceneMusicMapper;
    private SceneSwitch _sceneSwitch;
    private Dictionary<MusicCategory, IEnumerable<object>> _musicClipKeys;
    private AudioClip _pastClip = null;
    private CancellationTokenSource _cts;

    [Inject]
    public void Construct(IClipLoader clipLoader, MusicQueue musicQueue,
        ISceneMusicMapper sceneMusicMapper, SceneSwitch sceneSwitch)
    {
        _clipLoader = clipLoader;
        _musicQueue = musicQueue;
        _sceneMusicMapper = sceneMusicMapper;
        _sceneSwitch = sceneSwitch;
    }

    private void Awake() => _audioSource = GetComponent<AudioSource>();

    private async void Start()
    {
        _cts = new CancellationTokenSource();
        try
        {
            _musicClipKeys = await _clipLoader.LoadClipKeysAsync(_cts.Token);
        }
        catch (Exception)
        {
            return;
        }

        _sceneSwitch.IsSceneLoading
            .Where(value => !value)
            .Subscribe(value => TryPlayMusic())
            .AddTo(this);
    }

    private void OnDestroy()
    {
        _cts.CancelAndDispose();
        _cts = null;
    }

    private bool TryPlayMusic()
    {
        _cts.Cancel();
        _cts = new CancellationTokenSource();

        MusicCategory category = _sceneMusicMapper.GetMusicCategory(_sceneSwitch.CurrentSceneType);

        if (_musicClipKeys.TryGetValue(category, out IEnumerable<object> clipKeys))
        {
            PlayMusic(clipKeys).Forget();

            return true;
        }
        else
        {
            Debug.LogWarning($"No music found for category {category}");

            return false;
        }
    }

    private async UniTask PlayMusic(IEnumerable<object> clipKeys)
    {
        _musicQueue.SetClipKeys(clipKeys);
        _musicQueue.ShuffleClipKeys();

        while (!_cts.Token.IsCancellationRequested && isActiveAndEnabled)
            await PlayNextClipAsync();
    }

    private async UniTask PlayNextClipAsync()
    {
        if (_pastClip != null)
        {
            ReleaseClip(_pastClip);
            _pastClip = null;
        }

        object nextClipKey = _musicQueue.GetNextClipKey();

        if (nextClipKey == null)
            return;

        AudioClip clip = await _clipLoader.LoadClipAsync(nextClipKey, _cts.Token);
        _audioSource.clip = clip;
        _audioSource.Play();
        _pastClip = clip;

        await UniTask.WaitWhile(() => _audioSource.isPlaying, cancellationToken: _cts.Token);
    }

    private void ReleaseClip(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = null;
        _clipLoader.UnloadClip(clip);
        Resources.UnloadAsset(clip);
    }
}
