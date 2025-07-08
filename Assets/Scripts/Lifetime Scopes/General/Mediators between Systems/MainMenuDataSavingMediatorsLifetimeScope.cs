using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainMenuDataSavingMediatorsLifetimeScope : LifetimeScope
{
    [SerializeField] private WarningResetButtonView _warningResetButtonView;
    [SerializeField] private AchievedLevelButtonView _achievedLevelButtonView;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_warningResetButtonView);
        builder.RegisterInstance(_achievedLevelButtonView);

        builder.RegisterEntryPoint<WarningResetButtonViewSaveStorageMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<SaveStorageAchievedLevelButtonViewMediator>(Lifetime.Singleton);
    }
}
