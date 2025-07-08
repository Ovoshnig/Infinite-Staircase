using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainMenuLifetimeScope : LifetimeScope
{
    [SerializeField] private Canvas _mainMenuCanvas;

    protected override void Configure(IContainerBuilder builder)
    {
        PanelCloseButtonView[] panelCloseButtonViews = _mainMenuCanvas
            .GetComponentsInChildren<PanelCloseButtonView>(true);

        builder.RegisterInstance(panelCloseButtonViews);

        builder.RegisterEntryPoint<MenuInputHandlerPanelCloseButtonViewMediator>(Lifetime.Singleton);
    }

    private void Start() =>
        Container.InjectGameObject(_mainMenuCanvas.gameObject);
}
