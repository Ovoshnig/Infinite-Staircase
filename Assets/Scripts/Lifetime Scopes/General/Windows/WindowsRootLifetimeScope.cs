using VContainer;
using VContainer.Unity;

public class WindowsRootLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<WindowInputHandler>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<WindowTracker>(Lifetime.Singleton).AsSelf();
    }
}
