using R3;
using System;
using UnityEngine;
using VContainer.Unity;

public class GamePauser : IInitializable, IDisposable
{
    private readonly SceneSwitch _sceneSwitch;
    private readonly TimeSettings _timeSettings;
    private readonly Subject<bool> _isPause = new();

    public GamePauser(SceneSwitch sceneSwitch, 
        TimeSettings timeSettings)
    {
        _sceneSwitch = sceneSwitch;
        _timeSettings = timeSettings;
    }

    public Observable<bool> IsPause => _isPause;

    public void Initialize() => _sceneSwitch.SceneLoaded += OnSceneLoaded;

    public void Dispose() => _sceneSwitch.SceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(SceneSwitch.SceneType _) => Unpause();

    public void Pause() => SetPauseState(true);

    public void Unpause() => SetPauseState(false);

    private void SetPauseState(bool pause)
    {
        _isPause.OnNext(pause);
        Time.timeScale = pause ? _timeSettings.PauseTimeScale : _timeSettings.NormalTimeScale;
    }
}
