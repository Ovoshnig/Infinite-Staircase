using UnityEngine;
using VContainer;
using VContainer.Unity;

public class NewGameStartMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<NewGameStarter>(Lifetime.Singleton);

        Canvas canvas = FindFirstObjectByType<Canvas>();
        SeedInputFieldView seedInputFieldView = canvas
            .GetComponentInChildren<SeedInputFieldView>(true);

        if (seedInputFieldView != null)
        {
            builder.RegisterInstance(seedInputFieldView);
            builder.RegisterEntryPoint<NewGameStarterSeedInputFieldViewMediator>(Lifetime.Singleton);
        }

        FirstLevelButtonView firstLevelButtonView = canvas
            .GetComponentInChildren<FirstLevelButtonView>(true);

        if (firstLevelButtonView != null)
        {
            builder.RegisterInstance(firstLevelButtonView);
            builder.RegisterEntryPoint<NewGameStarterFirstLevelButtonViewMediator>(Lifetime.Singleton);
        }
    }
}
