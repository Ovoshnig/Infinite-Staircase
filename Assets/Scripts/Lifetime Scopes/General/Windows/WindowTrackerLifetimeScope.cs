using VContainer;
using VContainer.Unity;

public class WindowTrackerLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) => 
        builder.RegisterEntryPoint<WindowTracker>(Lifetime.Singleton).AsSelf();
}
