using VContainer;
using VContainer.Unity;

public class MenuInputHandlerLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) => 
        builder.RegisterEntryPoint<MenuInputHandler>(Lifetime.Singleton).AsSelf();
}
