using VContainer;
using VContainer.Unity;

public class MenuGameStartMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<GameStarter>(Lifetime.Singleton);
        builder.Register<SaveStorageNewGameViewMediatorFactory>(Lifetime.Singleton);
        builder.Register<GameStarterSeedViewMediatorFactory>(Lifetime.Singleton);
        builder.Register<GameStarterFirstLevelViewMediatorFactory>(Lifetime.Singleton);
    }

    private void Start()
    {
        SaveStorageNewGameViewMediatorFactory saveStorageNewGameViewMediatorFactory = Container
            .Resolve<SaveStorageNewGameViewMediatorFactory>();
        saveStorageNewGameViewMediatorFactory.CreateForEachView(Container);

        GameStarterSeedViewMediatorFactory gameStarterSeedViewMediatorFactory = Container
            .Resolve<GameStarterSeedViewMediatorFactory>();
        gameStarterSeedViewMediatorFactory.CreateForEachView(Container);

        GameStarterFirstLevelViewMediatorFactory gameStarterFirstLevelViewMediatorFactory = Container
            .Resolve<GameStarterFirstLevelViewMediatorFactory>();
        gameStarterFirstLevelViewMediatorFactory.CreateForEachView(Container);
    }
}
