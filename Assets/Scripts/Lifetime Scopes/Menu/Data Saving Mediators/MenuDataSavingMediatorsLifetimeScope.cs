using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MenuDataSavingMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<SaveStorageResetViewMediatorFactory>(Lifetime.Singleton);
        builder.Register<SaveStorageAchievedViewMediatorFactory>(Lifetime.Singleton);
    }

    private void Start()
    {
        Canvas canvas = Container.Resolve<Canvas>();

        SaveStorageResetViewMediatorFactory saveStorageResetViewMediatorFactory = Container
            .Resolve<SaveStorageResetViewMediatorFactory>();
        SaveResetButtonView[] saveResetButtonViews = canvas
            .GetComponentsInChildren<SaveResetButtonView>(true);

        foreach (var view in saveResetButtonViews)
            saveStorageResetViewMediatorFactory.Create(view);

        SaveStorageAchievedViewMediatorFactory saveStorageAchievedViewMediatorFactory = Container
            .Resolve<SaveStorageAchievedViewMediatorFactory>();
        AchievedLevelButtonView[] achievedLevelButtonViews = canvas
            .GetComponentsInChildren<AchievedLevelButtonView>(true);

        foreach (var view in achievedLevelButtonViews)
            saveStorageAchievedViewMediatorFactory.Create(view);
    }
}
