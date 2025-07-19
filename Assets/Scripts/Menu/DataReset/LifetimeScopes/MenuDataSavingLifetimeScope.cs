using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MenuDataSavingLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<SaveStorageResetViewMediatorFactory>(Lifetime.Singleton);
        builder.Register<SaveStorageAchievedViewMediatorFactory>(Lifetime.Singleton);
        builder.Register<SettingsStorageResetViewMediatorFactory>(Lifetime.Singleton);
    }

    private void Start()
    {
        Canvas canvas = Container.Resolve<Canvas>();

        SaveStorageResetViewMediatorFactory saveStorageResetViewMediatorFactory = Container
            .Resolve<SaveStorageResetViewMediatorFactory>();
        saveStorageResetViewMediatorFactory.CreateForEachView(canvas);

        SaveStorageAchievedViewMediatorFactory saveStorageAchievedViewMediatorFactory = Container
            .Resolve<SaveStorageAchievedViewMediatorFactory>();
        saveStorageAchievedViewMediatorFactory.CreateForEachView(canvas);

        SettingsStorageResetViewMediatorFactory settingsStorageResetViewMediatorFactory = Container
            .Resolve<SettingsStorageResetViewMediatorFactory>();
        settingsStorageResetViewMediatorFactory.CreateForEachView(canvas);
    }
}
