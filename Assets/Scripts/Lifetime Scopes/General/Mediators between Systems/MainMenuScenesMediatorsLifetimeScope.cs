using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainMenuScenesMediatorsLifetimeScope : LifetimeScope
{
    [SerializeField] private AchievedLevelButtonView _achievedLevelLoadView;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_achievedLevelLoadView);

        builder.RegisterEntryPoint<AchievedLevelButtonViewSceneSwitchMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<NewGameStarterSceneSwitchMediator>(Lifetime.Singleton);
    }
}
