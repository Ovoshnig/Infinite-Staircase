using VContainer;
using VContainer.Unity;

public class PauseMenuScenesMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register(resolver =>
        {
            PauseMenuSwitch pauseMenuSwitch = resolver.Resolve<PauseMenuSwitch>();
            return pauseMenuSwitch.GetComponentInChildren<CurrentLevelButtonView>(includeInactive: true);
        }, Lifetime.Singleton);

        builder.Register(resolver =>
        {
            PauseMenuSwitch pauseMenuSwitch = resolver.Resolve<PauseMenuSwitch>();
            return pauseMenuSwitch.GetComponentInChildren<MainMenuButtonView>(includeInactive: true);
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint<CurrentLevelButtonViewSceneSwitchMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<MainMenuButtonViewSceneSwitchMediator>(Lifetime.Singleton);
    }
}
