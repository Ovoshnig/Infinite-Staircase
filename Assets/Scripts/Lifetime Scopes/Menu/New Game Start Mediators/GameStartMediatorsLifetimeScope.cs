using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameStartMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<GameStarter>(Lifetime.Singleton);

        Canvas canvas = FindFirstObjectByType<Canvas>();
        SeedInputFieldView seedInputFieldView = canvas
            .GetComponentInChildren<SeedInputFieldView>(true);

        if (seedInputFieldView != null)
        {
            builder.RegisterInstance(seedInputFieldView);
            builder.RegisterEntryPoint<GameStarterSeedViewMediator>(Lifetime.Singleton);
        }

        FirstLevelButtonView firstLevelButtonView = canvas
            .GetComponentInChildren<FirstLevelButtonView>(true);

        if (firstLevelButtonView != null)
        {
            builder.RegisterInstance(firstLevelButtonView);
            builder.RegisterEntryPoint<GameStarterFirstLevelViewMediator>(Lifetime.Singleton);
        }
    }
}
