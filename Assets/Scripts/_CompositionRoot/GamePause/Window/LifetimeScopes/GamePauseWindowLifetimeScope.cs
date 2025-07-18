using VContainer;
using VContainer.Unity;

public class GamePauseWindowLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) => 
        builder.RegisterEntryPoint<GamePauserPauseMenuWindowMediator>(Lifetime.Singleton);
}
