using VContainer;
using VContainer.Unity;

public class MenuDataSavingMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<SaveStorageResetViewMediatorFactory>(Lifetime.Singleton);
        builder.Register<SaveStorageAchievedViewMediatorFactory>(Lifetime.Singleton);
        builder.Register<SettingsStorageResetViewMediatorFactory>(Lifetime.Singleton);
    }

    private void Start()
    {
        SaveStorageResetViewMediatorFactory saveStorageResetViewMediatorFactory = Container
            .Resolve<SaveStorageResetViewMediatorFactory>();
        saveStorageResetViewMediatorFactory.CreateForEachView(Container);

        SaveStorageAchievedViewMediatorFactory saveStorageAchievedViewMediatorFactory = Container
            .Resolve<SaveStorageAchievedViewMediatorFactory>();
        saveStorageAchievedViewMediatorFactory.CreateForEachView(Container);

        SettingsStorageResetViewMediatorFactory settingsStorageResetViewMediatorFactory = Container
            .Resolve<SettingsStorageResetViewMediatorFactory>();
        settingsStorageResetViewMediatorFactory.CreateForEachView(Container);
    }
}
