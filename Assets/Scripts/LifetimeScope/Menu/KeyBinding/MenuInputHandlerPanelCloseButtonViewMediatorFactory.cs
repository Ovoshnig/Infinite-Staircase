public class MenuInputHandlerPanelCloseButtonViewMediatorFactory
    : MediatorFactory<MenuInputHandlerPanelCloseButtonViewMediator, KeyListeningTracker, PanelCloseButtonView[]>
{
    private readonly MenuInputHandler _menuInputHandler;

    public MenuInputHandlerPanelCloseButtonViewMediatorFactory(MenuInputHandler menuInputHandler) =>
        _menuInputHandler = menuInputHandler;

    protected override MenuInputHandlerPanelCloseButtonViewMediator CreateMediatorInstance(
        KeyListeningTracker keyListeningTracker, PanelCloseButtonView[] views) =>
        new(_menuInputHandler, views, keyListeningTracker);
}
