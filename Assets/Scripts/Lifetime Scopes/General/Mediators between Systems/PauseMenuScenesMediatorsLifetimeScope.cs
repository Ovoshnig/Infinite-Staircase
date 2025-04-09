using VContainer;
using VContainer.Unity;

public class PauseMenuScenesMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register(resolver =>
        {
            WindowView windowView = resolver.Resolve<WindowView>();
            return windowView.GetComponentInChildren<CurrentLevelButtonView>(includeInactive: true);
        }, Lifetime.Singleton);

        builder.Register(resolver =>
        {
            WindowView windowView = resolver.Resolve<WindowView>();
            return windowView.GetComponentInChildren<MainMenuButtonView>(includeInactive: true);
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint<CurrentLevelButtonViewSceneSwitchMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<MainMenuButtonViewSceneSwitchMediator>(Lifetime.Singleton);
    }
}
