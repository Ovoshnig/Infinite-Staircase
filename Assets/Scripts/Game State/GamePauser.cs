using R3;
using System;
using UnityEngine;
using Zenject;

public class GamePauser : IInitializable, IDisposable
{
    private readonly SceneSwitch _sceneSwitch;
    private GameSettingsInstaller.TimeSettings _timeSettings;
    private readonly Subject<bool> _isPause = new();

    public GamePauser(SceneSwitch sceneSwitch, 
        GameSettingsInstaller.TimeSettings timeSettings)
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
