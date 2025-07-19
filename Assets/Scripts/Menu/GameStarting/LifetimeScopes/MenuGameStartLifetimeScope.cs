using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MenuGameStartLifetimeScope : LifetimeScope
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
        Canvas canvas = Container.Resolve<Canvas>();

        SaveStorageNewGameViewMediatorFactory saveStorageNewGameViewMediatorFactory = Container
            .Resolve<SaveStorageNewGameViewMediatorFactory>();
        saveStorageNewGameViewMediatorFactory.CreateForEachView(canvas);

        GameStarterSeedViewMediatorFactory gameStarterSeedViewMediatorFactory = Container
            .Resolve<GameStarterSeedViewMediatorFactory>();
        gameStarterSeedViewMediatorFactory.CreateForEachView(canvas);

        GameStarterFirstLevelViewMediatorFactory gameStarterFirstLevelViewMediatorFactory = Container
            .Resolve<GameStarterFirstLevelViewMediatorFactory>();
        gameStarterFirstLevelViewMediatorFactory.CreateForEachView(canvas);
    }
}
