using System;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SplashScreenPasser : IDisposable
{
    private PlayerInput _playerInput;

    public void Dispose() => _playerInput.Disable();

    private SplashScreenPasser()
    {
#if !UNITY_EDITOR
        PlaySplashScreen();
#endif
        InitializePlayerInput();
    }

    private void PlaySplashScreen()
    {
        SplashScreen.Begin();
        SplashScreen.Draw();
    }

    private void InitializePlayerInput()
    {
        _playerInput = new PlayerInput();
        _playerInput.SplashScreen.Pass.performed += Pass;
        _playerInput.Enable();
    }

    private void Pass(InputAction.CallbackContext _)
    {
        if (!SplashScreen.isFinished)
            SplashScreen.Stop(SplashScreen.StopBehavior.FadeOut);
    }
}
