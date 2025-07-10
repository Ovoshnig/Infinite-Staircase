using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PauseMenuScenesMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register(resolver =>
        {
            Canvas windowCanvas = resolver.Resolve<Canvas>();
            return windowCanvas.GetComponentInChildren<CurrentLevelButtonView>(includeInactive: true);
        }, Lifetime.Singleton);

        builder.Register(resolver =>
        {
            Canvas windowCanvas = resolver.Resolve<Canvas>();
            return windowCanvas.GetComponentInChildren<MainMenuButtonView>(includeInactive: true);
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint<SceneSwitchCurrentViewMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<SceneSwitchMenuViewMediator>(Lifetime.Singleton);
    }
}
