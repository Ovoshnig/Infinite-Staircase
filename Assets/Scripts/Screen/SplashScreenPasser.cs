using R3;
using System;
using UnityEngine.Rendering;
using Zenject;

public class SplashScreenPasser : IInitializable, IDisposable
{
    private readonly ScreenInputHandler _inputHandler;
    private IDisposable _disposed;

    [Inject]
    public SplashScreenPasser(ScreenInputHandler screenInputHandler) => _inputHandler = screenInputHandler;

    public void Initialize()
    {
        Play();

        _disposed = _inputHandler.IsPassSplashImagePressed
            .Where(value => value)
            .Subscribe(_ => Pass());
    }

    public void Dispose() => _disposed?.Dispose();

    private void Play()
    {
        SplashScreen.Begin();
        SplashScreen.Draw();
    }

    private void Pass()
    {
        if (!SplashScreen.isFinished)
            SplashScreen.Stop(SplashScreen.StopBehavior.FadeOut);
    }
}
