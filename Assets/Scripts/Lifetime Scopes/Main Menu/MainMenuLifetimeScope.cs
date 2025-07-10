using VContainer;
using VContainer.Unity;

public class MainMenuLifetimeScope : LifetimeScope
{
    private MainMenuCanvasView _mainMenuCanvasView;

    protected override void Configure(IContainerBuilder builder)
    {
        _mainMenuCanvasView = FindFirstObjectByType<MainMenuCanvasView>();
        PanelCloseButtonView[] panelCloseButtonViews = _mainMenuCanvasView
            .GetComponentsInChildren<PanelCloseButtonView>(true);

        builder.RegisterInstance(panelCloseButtonViews);

        builder.RegisterEntryPoint<MenuInputHandler>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<MenuInputHandlerPanelCloseButtonViewMediator>(Lifetime.Singleton);
    }

    private void Start() =>
        Container.InjectGameObject(_mainMenuCanvasView.gameObject);
}
