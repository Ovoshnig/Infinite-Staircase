using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class MusicPlayer : IDisposable
{
    private enum MusicCategory
    {
        MainMenu,
        GameLevels,
        Credits
    }

    private const string MusicClipsPath = "Audio/Music";

    private readonly System.Random _random = new();
    private readonly SceneSwitch _sceneSwitch;
    private readonly AudioSource _musicSource;
    private List<AudioClip> _currentTracks;
    private readonly Dictionary<MusicCategory, List<AudioClip>> _musicTracks;
    private Queue<AudioClip> _trackQueue;
    private CancellationTokenSource _cts = new();

    public event Action MusicTrackChanged;

    [Inject]
    public MusicPlayer([Inject(Id = "musicSource")] AudioSource musicSource, SceneSwitch sceneSwitch)
    {
        _musicSource = musicSource;
        _sceneSwitch = sceneSwitch;
        _sceneSwitch.SceneLoaded += OnLevelLoaded;

        _musicTracks = new Dictionary<MusicCategory, List<AudioClip>>();
        LoadMusicTracks();
    }

    public void Dispose()
    {
        _sceneSwitch.SceneLoaded -= OnLevelLoaded;
        CancelToken();
    }

    private void OnLevelLoaded(SceneSwitch.Scene scene)
    {
        Dictionary<SceneSwitch.Scene, MusicCategory> sceneToMusicCategory = new()
        {
            { SceneSwitch.Scene.MainMenu, MusicCategory.MainMenu },
            { SceneSwitch.Scene.GameLevel, MusicCategory.GameLevels },
            { SceneSwitch.Scene.Credits, MusicCategory.Credits }
        };

        if (scene == SceneSwitch.Scene.GameLevel && _currentTracks == _musicTracks[MusicCategory.GameLevels])
            return;

        if (sceneToMusicCategory.TryGetValue(scene, out MusicCategory category))
            SetCurrentTracks(category);
        else
            SetCurrentTracks(MusicCategory.MainMenu);

        CancelToken();
        _cts = new CancellationTokenSource();

        if (_currentTracks.Count > 0)
        {
            ShuffleAndQueueTracks();
            PlayNextTrack().Forget();
        }
    }

    private void CancelToken()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }

    private AudioClip GetNextTrack()
    {
        if (_trackQueue.Count == 0)
            ShuffleAndQueueTracks();

        return _trackQueue.Dequeue();
    }

    private void LoadMusicTracks()
    {
        foreach (MusicCategory category in Enum.GetValues(typeof(MusicCategory)))
        {
            var tracks = new List<AudioClip>(Resources.LoadAll<AudioClip>($"{MusicClipsPath}/{category}"));
            _musicTracks[category] = tracks;

            if (tracks.Count == 0)
                Debug.LogWarning($"No music tracks found in Resources/{MusicClipsPath}/{category}.");
        }
    }

    private void SetCurrentTracks(MusicCategory category)
    {
        _currentTracks = _musicTracks[category];
    }

    private void ShuffleAndQueueTracks()
    {
        List<AudioClip> tracks = new(_currentTracks);
        _trackQueue = new Queue<AudioClip>();

        while (tracks.Count > 0)
        {
            int index = _random.Next(tracks.Count);
            _trackQueue.Enqueue(tracks[index]);
            tracks.RemoveAt(index);
        }
    }

    private async UniTask PlayNextTrack()
    {
        while (true)
        {
            AudioClip clip = GetNextTrack();
            _musicSource.clip = clip;
            _musicSource.Play();
            MusicTrackChanged?.Invoke();
            await UniTask.WaitWhile(() => _musicSource.isPlaying, cancellationToken: _cts.Token);
        }
    }
}
