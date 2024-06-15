using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GamePauser : IDisposable
{
    private readonly PlayerInput _playerInput;
    private readonly SceneSwitch _sceneSwitch;
    private bool _isGamePaused = false;
    private bool _reversePauseStateAllowed = true;

    public event Action GamePaused;
    public event Action GameUnpaused;

    [Inject]
    public GamePauser(SceneSwitch sceneSwitch)
    {
        _sceneSwitch = sceneSwitch;
        _sceneSwitch.SceneLoading += OnLevelLoading;
        _sceneSwitch.SceneLoaded += OnLevelLoaded;

        _playerInput = new PlayerInput();
        _playerInput.GameState.ReversePauseState.performed += ReversePauseState;
        _playerInput.Enable();

        SetPauseState(pause: false);
    }

    public void Dispose()
    {
        _sceneSwitch.SceneLoading -= OnLevelLoading;
        _sceneSwitch.SceneLoaded -= OnLevelLoaded;
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

    private void OnLevelLoading(SceneSwitch.Scene scene)
    {
        Unpause();
        _reversePauseStateAllowed = false;
    }

    private void OnLevelLoaded(SceneSwitch.Scene scene)
    {
        _reversePauseStateAllowed = true;
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
