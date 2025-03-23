using R3;
using System;
using UnityEngine.Rendering;
using VContainer.Unity;

public class SplashScreenPasser : IInitializable, IDisposable
{
    private readonly ScreenInputHandler _screenInputHandler;
    private readonly CompositeDisposable _compositeDisposable = new();

    public SplashScreenPasser(ScreenInputHandler screenInputHandler) => _screenInputHandler = screenInputHandler;

    public void Initialize()
    {
        Play();

        _screenInputHandler.IsPassSplashImagePressed
            .Where(value => value)
            .Subscribe(_ => OnPassPressed())
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();

    private void Play()
    {
        SplashScreen.Begin();
        SplashScreen.Draw();
    }

    private void OnPassPressed()
    {
        if (!SplashScreen.isFinished)
            SplashScreen.Stop(SplashScreen.StopBehavior.FadeOut);
    }
}
