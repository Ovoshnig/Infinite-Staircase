using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GamePauser : IInitializable, IDisposable
{
    private readonly SceneSwitch _sceneSwitch;
    private readonly PlayerInput _playerInput = new();
    private bool _isGamePaused = false;
    private bool _reversePauseStateAllowed = true;

    public event Action GamePaused;
    public event Action GameUnpaused;

    [Inject]
    public GamePauser(SceneSwitch sceneSwitch) => _sceneSwitch = sceneSwitch;

    public void Initialize()
    {
        /*_sceneSwitch.SceneLoading += OnSceneLoading;
        _sceneSwitch.SceneLoaded += OnSceneLoaded;

        _playerInput.PauseMenu.OpenOrClose.performed += ReversePauseState;
        _playerInput.Enable();

        SetPauseState(pause: false);*/
    }

    public void Dispose()
    {
        _sceneSwitch.SceneLoading -= OnSceneLoading;
        _sceneSwitch.SceneLoaded -= OnSceneLoaded;

        _playerInput.Disable();
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

    private void OnSceneLoading(SceneSwitch.SceneType scene)
    {
        Unpause();
        _reversePauseStateAllowed = false;
    }

    private void OnSceneLoaded(SceneSwitch.SceneType scene)
    {
        _reversePauseStateAllowed = true;

        PauseMenu pauseMenuHandler = UnityEngine.Object.FindFirstObjectByType<PauseMenu>();

        if (pauseMenuHandler != null)
            pauseMenuHandler.ResumeClicked += Unpause;
    }

    private void SetPauseState(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
        _isGamePaused = pause;
    }

    private void ReversePauseState(InputAction.CallbackContext context)
    {
        if (_reversePauseStateAllowed)
        {
            if (_isGamePaused)
                Unpause();
            else
                Pause();
        }
    }
}
