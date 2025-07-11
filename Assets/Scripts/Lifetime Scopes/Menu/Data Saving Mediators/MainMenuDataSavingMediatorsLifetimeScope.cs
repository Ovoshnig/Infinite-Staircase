using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainMenuDataSavingMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        WarningResetButtonView warningResetButtonView = canvas
            .GetComponentInChildren<WarningResetButtonView>(true);

        if (warningResetButtonView != null)
        {
            builder.RegisterInstance(warningResetButtonView);
            builder.RegisterEntryPoint<SaveStorageWarningResetButtonViewMediator>(Lifetime.Singleton);
        }

        AchievedLevelButtonView achievedLevelButtonView = canvas
            .GetComponentInChildren<AchievedLevelButtonView>(true);

        if (achievedLevelButtonView != null)
        {
            builder.RegisterInstance(achievedLevelButtonView);
            builder.RegisterEntryPoint<SaveStorageAchievedLevelButtonViewMediator>(Lifetime.Singleton);
        }
    }
}
