using VContainer;
using VContainer.Unity;

public sealed class PauseMenuWindowLifetimeScope : WindowLifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<PauseMenuWindow>(Lifetime.Singleton).AsSelf().As<Window>();

        base.Configure(builder);
    }
}
