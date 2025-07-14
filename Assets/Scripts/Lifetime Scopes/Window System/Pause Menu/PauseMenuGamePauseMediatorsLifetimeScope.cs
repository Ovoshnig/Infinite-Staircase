using VContainer;
using VContainer.Unity;

public class PauseMenuGamePauseMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) => 
        builder.RegisterEntryPoint<PauseMenuWindowGamePauserMediator>(Lifetime.Singleton);
}
