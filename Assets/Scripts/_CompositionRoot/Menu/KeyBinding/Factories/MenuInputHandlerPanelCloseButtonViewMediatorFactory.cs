public class MenuInputHandlerPanelCloseButtonViewMediatorFactory
    : MediatorFactory<MenuInputHandlerPanelCloseButtonViewMediator, ButtonListener, PanelCloseButtonView[]>
{
    private readonly MenuInputHandler _menuInputHandler;

    public MenuInputHandlerPanelCloseButtonViewMediatorFactory(MenuInputHandler menuInputHandler) =>
        _menuInputHandler = menuInputHandler;

    protected override MenuInputHandlerPanelCloseButtonViewMediator CreateMediatorInstance(
        ButtonListener buttonListener, PanelCloseButtonView[] views) =>
        new(_menuInputHandler, views, buttonListener);
}
