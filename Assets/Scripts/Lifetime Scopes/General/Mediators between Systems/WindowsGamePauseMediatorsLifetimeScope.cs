using VContainer;
using VContainer.Unity;

public class WindowsGamePauseMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<PauseMenuSwitchGamePauserMediator>(Lifetime.Singleton);
    }
}
