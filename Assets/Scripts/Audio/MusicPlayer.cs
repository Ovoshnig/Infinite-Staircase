using Cysharp.Threading.Tasks;
using Random = System.Random;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    private enum MusicCategory
    {
        MainMenu,
        GameLevels,
        Credits
    }

    private const string MusicClipsPath = "Audio/Music";

    private readonly Random _random = new();
    private SceneSwitch _sceneSwitch;
    private AudioSource _audioSource;
    private Dictionary<MusicCategory, List<AudioClip>> _musicTracks;
    private List<AudioClip> _currentTracks;
    private Queue<AudioClip> _trackQueue;
    private CancellationTokenSource _cts = new();

    public event Action MusicTrackChanged;

    [Inject]
    private void Construct(SceneSwitch sceneSwitch) => _sceneSwitch = sceneSwitch;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _musicTracks = new Dictionary<MusicCategory, List<AudioClip>>();
    }

    private void Start() => LoadMusicTracks();

    private void OnEnable() => _sceneSwitch.SceneLoaded += OnSceneLoaded;

    public void OnDisable()
    {
        _sceneSwitch.SceneLoaded -= OnSceneLoaded;
        CancelToken();
    }

    private void OnSceneLoaded(SceneSwitch.Scene scene)
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

    private void SetCurrentTracks(MusicCategory category) => _currentTracks = _musicTracks[category];

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
            _audioSource.clip = clip;
            _audioSource.Play();
            MusicTrackChanged?.Invoke();
            await UniTask.WaitWhile(() => _audioSource.isPlaying, cancellationToken: _cts.Token);
        }
    }
}
