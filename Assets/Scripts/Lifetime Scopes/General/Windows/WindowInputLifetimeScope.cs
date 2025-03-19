using VContainer;
using VContainer.Unity;

public class WindowInputLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) => 
        builder.RegisterEntryPoint<WindowInputHandler>(Lifetime.Singleton).AsSelf();
}
