using VContainer;
using VContainer.Unity;

public class PlayerGamePauseMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) =>
        builder.RegisterEntryPoint<GamePauserPlayerMoverMediator>(Lifetime.Singleton);
}
