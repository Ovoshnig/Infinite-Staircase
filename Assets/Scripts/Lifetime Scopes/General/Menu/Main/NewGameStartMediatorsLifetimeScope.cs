using VContainer;
using VContainer.Unity;

public class NewGameStartMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<NewGameStarter>(Lifetime.Singleton);

        MainMenuCanvasView canvasView = FindFirstObjectByType<MainMenuCanvasView>();
        SeedInputFieldView seedInputFieldView = canvasView
            .GetComponentInChildren<SeedInputFieldView>(true);

        if (seedInputFieldView != null)
        {
            builder.RegisterInstance(seedInputFieldView);
            builder.RegisterEntryPoint<NewGameStarterSeedInputFieldViewMediator>(Lifetime.Singleton);
        }

        FirstLevelButtonView firstLevelButtonView = canvasView
            .GetComponentInChildren<FirstLevelButtonView>(true);

        if (firstLevelButtonView != null)
        {
            builder.RegisterInstance(firstLevelButtonView);
            builder.RegisterEntryPoint<NewGameStarterFirstLevelButtonViewMediator>(Lifetime.Singleton);
        }
    }
}
