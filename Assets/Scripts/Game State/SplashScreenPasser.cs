using System;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Zenject;

public class SplashScreenPasser : IInitializable, IDisposable
{
    private readonly PlayerInput _playerInput = new();

    public void Initialize()
    {
        PlaySplashScreen();

        _playerInput.SplashScreen.Pass.performed += OnPassPerformed;
        _playerInput.Enable();
    }

    public void Dispose() => _playerInput.Disable();

    private void PlaySplashScreen()
    {
        SplashScreen.Begin();
        SplashScreen.Draw();
    }

    private void OnPassPerformed(InputAction.CallbackContext _)
    {
        if (!SplashScreen.isFinished)
            SplashScreen.Stop(SplashScreen.StopBehavior.FadeOut);
    }
}
