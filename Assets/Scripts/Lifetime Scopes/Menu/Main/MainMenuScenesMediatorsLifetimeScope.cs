using VContainer;
using VContainer.Unity;

public class MainMenuScenesMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<NewGameStarterSceneSwitchMediator>(Lifetime.Singleton);

        MainMenuCanvasView canvasView = FindFirstObjectByType<MainMenuCanvasView>();
        AchievedLevelButtonView achievedLevelButtonView = canvasView
            .GetComponentInChildren<AchievedLevelButtonView>(true);

        if (achievedLevelButtonView != null)
        {
            builder.RegisterInstance(achievedLevelButtonView);
            builder.RegisterEntryPoint<SceneSwitchAchievedViewMediator>(Lifetime.Singleton);
        }
    }
}
