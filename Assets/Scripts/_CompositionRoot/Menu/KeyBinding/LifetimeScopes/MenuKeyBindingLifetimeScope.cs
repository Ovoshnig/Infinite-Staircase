using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MenuKeyBindingLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) =>
        builder.Register<MenuInputHandlerPanelCloseButtonViewMediatorFactory>(Lifetime.Singleton);

    private void Start()
    {
        Canvas windowCanvas = Container.Resolve<Canvas>();
        PanelCloseButtonView[] views = windowCanvas
            .GetComponentsInChildren<PanelCloseButtonView>(includeInactive: true);

        KeyListeningTracker keyListeningTracker =
            Container.TryResolve(out KeyListeningTracker tracker) ? tracker : null;

        MenuInputHandlerPanelCloseButtonViewMediatorFactory factory =
            Container.Resolve<MenuInputHandlerPanelCloseButtonViewMediatorFactory>();
        factory.Create(keyListeningTracker, views);
    }
}
