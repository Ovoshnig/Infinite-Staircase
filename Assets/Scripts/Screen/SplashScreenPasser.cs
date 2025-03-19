using R3;
using System;
using UnityEngine.Rendering;
using VContainer.Unity;

public class SplashScreenPasser : IInitializable, IDisposable
{
    private readonly ScreenInputHandler _inputHandler;
    private readonly CompositeDisposable _compositeDisposable = new();

    public SplashScreenPasser(ScreenInputHandler inputHandler) => _inputHandler = inputHandler;

    public void Initialize()
    {
        Play();

        _inputHandler.IsPassSplashImagePressed
            .Where(value => value)
            .Subscribe(_ => Pass())
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();

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
