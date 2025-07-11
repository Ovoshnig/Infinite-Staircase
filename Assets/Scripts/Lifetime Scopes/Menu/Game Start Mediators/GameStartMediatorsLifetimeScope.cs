using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameStartMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<GameStarter>(Lifetime.Singleton);
        builder.Register<GameStarterSeedViewMediatorFactory>(Lifetime.Singleton);
        builder.Register<GameStarterFirstLevelViewMediatorFactory>(Lifetime.Singleton);
    }

    private void Start()
    {
        Canvas canvas = Container.Resolve<Canvas>();

        SeedInputFieldView[] seedViews = canvas.GetComponentsInChildren<SeedInputFieldView>(true);
        GameStarterSeedViewMediatorFactory gameStarterSeedViewMediatorFactory = Container
            .Resolve<GameStarterSeedViewMediatorFactory>();

        foreach (var seedView in seedViews)
            gameStarterSeedViewMediatorFactory.Create(seedView);

        FirstLevelButtonView[] firstLevelViews = canvas
            .GetComponentsInChildren<FirstLevelButtonView>(true);
        GameStarterFirstLevelViewMediatorFactory gameStarterFirstLevelViewMediatorFactory = Container
            .Resolve<GameStarterFirstLevelViewMediatorFactory>();

        foreach (var firstLevelView in firstLevelViews)
            gameStarterFirstLevelViewMediatorFactory.Create(firstLevelView);
    }
}
