public class MenuInputHandlerPanelCloseButtonViewMediatorFactory
    : MediatorFactory<MenuInputHandlerPanelCloseButtonViewMediator, KeyListeningTracker, PanelCloseButtonView[]>
{
    private readonly MenuInputHandler _menuInputHandler;

    public MenuInputHandlerPanelCloseButtonViewMediatorFactory(MenuInputHandler menuInputHandler) =>
        _menuInputHandler = menuInputHandler;

    public override MenuInputHandlerPanelCloseButtonViewMediator
        Create(KeyListeningTracker keyListeningTracker, PanelCloseButtonView[] views)
    {
        MenuInputHandlerPanelCloseButtonViewMediator mediator = new(_menuInputHandler, views, keyListeningTracker);
        mediator.Initialize();
        Disposables.Add(mediator);
        return mediator;
    }
}
