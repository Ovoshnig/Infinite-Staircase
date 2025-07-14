using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer.Unity;

public class MusicPlayer : IDisposable
{
    private readonly MusicQueue _musicQueue;
    private readonly IClipLoader _clipLoader;
    private readonly ISceneMusicMapper _sceneMusicMapper;
    private readonly SceneSwitch _sceneSwitch;
    private readonly Subject<AudioClip> _playbackStarted = new();
    private readonly Subject<Unit> _playbackEnded = new();

    private Dictionary<MusicCategory, IEnumerable<object>> _musicClipKeys = null;
    private AudioClip _pastClip = null;
    private CancellationTokenSource _cts = new();

    public MusicPlayer(IClipLoader clipLoader, MusicQueue musicQueue,
        ISceneMusicMapper sceneMusicMapper, SceneSwitch sceneSwitch)
    {
        _clipLoader = clipLoader;
        _musicQueue = musicQueue;
        _sceneMusicMapper = sceneMusicMapper;
        _sceneSwitch = sceneSwitch;
    }

    public Observable<AudioClip> PlaybackStarted => _playbackStarted;
    public Observable<Unit> PlaybackEnded => _playbackEnded;

    public void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    public async UniTask LoadClipKeysAsync()
    {
        try
        {
            _musicClipKeys ??= await _clipLoader.LoadClipKeysAsync(_cts.Token);
            TryPlayMusic();
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    private bool TryPlayMusic()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        MusicCategory category = _sceneMusicMapper.GetMusicCategory(_sceneSwitch.CurrentSceneType);

        if (_musicClipKeys.TryGetValue(category, out IEnumerable<object> clipKeys))
        {
            PlayMusicAsync(clipKeys).Forget();
            return true;
        }
        else
        {
            Debug.LogWarning($"No music found for category {category}");
            return false;
        }
    }

    private async UniTask PlayMusicAsync(IEnumerable<object> clipKeys)
    {
        _musicQueue.SetClipKeys(clipKeys);
        _musicQueue.ShuffleClipKeys();

        while (!_cts.Token.IsCancellationRequested)
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
        _playbackStarted.OnNext(clip);
        _pastClip = clip;

        await UniTask.WaitForSeconds(clip.length, cancellationToken: _cts.Token);
    }

    private void ReleaseClip(AudioClip clip)
    {
        _playbackEnded.OnNext(Unit.Default);
        _clipLoader.UnloadClip(clip);
        Resources.UnloadAsset(clip);
    }
}
