using VContainer;
using VContainer.Unity;

public class MainMenuDataSavingMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        MainMenuCanvasView canvasView = FindFirstObjectByType<MainMenuCanvasView>();
        WarningResetButtonView warningResetButtonView = canvasView
            .GetComponentInChildren<WarningResetButtonView>(true);

        if (warningResetButtonView != null)
        {
            builder.RegisterInstance(warningResetButtonView);
            builder.RegisterEntryPoint<SaveStorageWarningResetButtonViewMediator>(Lifetime.Singleton);
        }

        AchievedLevelButtonView achievedLevelButtonView = canvasView
            .GetComponentInChildren<AchievedLevelButtonView>(true);

        if (achievedLevelButtonView != null)
        {
            builder.RegisterInstance(achievedLevelButtonView);
            builder.RegisterEntryPoint<SaveStorageAchievedLevelButtonViewMediator>(Lifetime.Singleton);
        }
    }
}
