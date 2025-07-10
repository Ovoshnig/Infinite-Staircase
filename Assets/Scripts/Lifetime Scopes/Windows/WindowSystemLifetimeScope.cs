using VContainer;
using VContainer.Unity;

public class WindowSystemLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<WindowTracker>(Lifetime.Singleton);
        builder.Register<CursorTuner>(Lifetime.Singleton);

        builder.RegisterEntryPoint<MenuInputHandler>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<WindowInputHandler>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<WindowTrackerCursorTunerMediator>(Lifetime.Singleton).AsSelf();
    }
}
