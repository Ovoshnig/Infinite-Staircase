using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MusicPlayer : IDisposable
{
    private readonly MusicQueue _musicQueue;
    private readonly IClipLoader _clipLoader;
    private readonly ISceneMusicMapper _sceneMusicMapper;
    private readonly Subject<AudioClip> _playbackStarted = new();
    private readonly Subject<Unit> _playbackEnded = new();

    private Dictionary<MusicCategory, IEnumerable<object>> _musicClipKeys = null;
    private AudioClip _pastClip = null;
    private CancellationTokenSource _cts = new();

    public MusicPlayer(IClipLoader clipLoader, MusicQueue musicQueue,
        ISceneMusicMapper sceneMusicMapper)
    {
        _clipLoader = clipLoader;
        _musicQueue = musicQueue;
        _sceneMusicMapper = sceneMusicMapper;
    }

    public Observable<AudioClip> PlaybackStarted => _playbackStarted;
    public Observable<Unit> PlaybackEnded => _playbackEnded;

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    public async UniTask StartPlayMusicAsync(SceneSwitch.SceneType sceneType)
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();

        try
        {
            _musicClipKeys ??= await _clipLoader.LoadClipKeysAsync(_cts.Token);

            MusicCategory category = _sceneMusicMapper.GetMusicCategory(sceneType);

            if (_musicClipKeys.TryGetValue(category, out IEnumerable<object> clipKeys))
                await PlayMusicAsync(clipKeys, _cts.Token);
            else
                Debug.LogWarning($"No music found for category {category}");
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    private async UniTask PlayMusicAsync(IEnumerable<object> clipKeys,
        CancellationToken token)
    {
        _musicQueue.Clear();

        while (!token.IsCancellationRequested)
        {
            if (_musicQueue.TryGetNextClipKey(out object clipKey))
            {
                await PlayNextClipAsync(clipKey, token);
            }
            else
            {
                _musicQueue.SetClipKeys(clipKeys);
                _musicQueue.ShuffleClipKeys();
            }
        }
    }

    private async UniTask PlayNextClipAsync(object clipKey, CancellationToken token)
    {
        if (_pastClip != null)
        {
            ReleaseClip(_pastClip);
            _pastClip = null;
        }

        AudioClip clip = await _clipLoader.LoadClipAsync(clipKey, token);
        _playbackStarted.OnNext(clip);
        _pastClip = clip;

        await UniTask.WaitForSeconds(clip.length, cancellationToken: token);
    }

    private void ReleaseClip(AudioClip clip)
    {
        _playbackEnded.OnNext(Unit.Default);
        _clipLoader.UnloadClip(clip);
        Resources.UnloadAsset(clip);
    }
}
