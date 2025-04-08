using UnityEngine;
using VContainer;
using VContainer.Unity;

public class NewGameStartMediatorsLifetimeScope : LifetimeScope
{
    [SerializeField] private SeedInputFieldView _seedInputView;
    [SerializeField] private FirstLevelButtonView _firstLevelLoadView;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_seedInputView);
        builder.RegisterInstance(_firstLevelLoadView);

        builder.Register<NewGameStarter>(Lifetime.Singleton);

        builder.RegisterEntryPoint<SeedInputFieldNewGameStarterMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<ButtonViewNewGameStarterMediator>(Lifetime.Singleton);
    }
}
