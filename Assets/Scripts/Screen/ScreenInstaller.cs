using VContainer;
using VContainer.Unity;

public class ScreenInstaller : IInstaller
{
    public void Install(IContainerBuilder builder)
    {
#if !UNITY_EDITOR
        builder.RegisterEntryPoint<SplashScreenPasser>(Lifetime.Singleton).AsSelf();
#endif
        builder.RegisterEntryPoint<ScreenInputHandler>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<FullScreenTuner>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<VSyncTuner>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<ResolutionTuner>(Lifetime.Singleton).AsSelf();
    }
}
