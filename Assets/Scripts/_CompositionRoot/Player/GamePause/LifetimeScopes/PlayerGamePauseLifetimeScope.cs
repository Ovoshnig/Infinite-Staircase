using VContainer;
using VContainer.Unity;

public class PlayerGamePauseLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) =>
        builder.RegisterEntryPoint<PlayerMoverGamePauserMediator>(Lifetime.Singleton);
}
