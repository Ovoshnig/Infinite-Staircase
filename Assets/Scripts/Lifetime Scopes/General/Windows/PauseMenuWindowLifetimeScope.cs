using VContainer;
using VContainer.Unity;

public sealed class PauseMenuWindowLifetimeScope : WindowLifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<PauseMenuSwitch>(Lifetime.Singleton).AsSelf().As<WindowSwitch>();

        base.Configure(builder);
    }
}
