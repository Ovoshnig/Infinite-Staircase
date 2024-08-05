using System;
using UnityEngine;
using Zenject;

public class GamePauser : IInitializable, IDisposable
{
    private readonly PauseMenu _pauseMenu;
    private readonly SceneSwitch _sceneSwitch;

    public event Action GamePaused;
    public event Action GameUnpaused;

    [Inject]
    public GamePauser(PauseMenu pauseMenu, SceneSwitch sceneSwitch)
    {
        _pauseMenu = pauseMenu;
        _sceneSwitch = sceneSwitch;
    }

    public void Initialize()
    {
        _pauseMenu.Paused += Pause;
        _pauseMenu.Resumed += Unpause;
        _sceneSwitch.SceneLoading += OnSceneLoading;

        SetPauseState(pause: false);
    }

    public void Dispose()
    {
        _pauseMenu.Paused -= Pause;
        _pauseMenu.Resumed -= Unpause;
        _sceneSwitch.SceneLoading -= OnSceneLoading;
    }

    private void Pause()
    {
        SetPauseState(pause: true);
        GamePaused?.Invoke();
    }

    private void Unpause()
    {
        SetPauseState(pause: false);
        GameUnpaused?.Invoke();
    }

    private void OnSceneLoading(SceneSwitch.SceneType _) => Unpause();

    private void SetPauseState(bool pause) => Time.timeScale = pause ? 0 : 1;
}
