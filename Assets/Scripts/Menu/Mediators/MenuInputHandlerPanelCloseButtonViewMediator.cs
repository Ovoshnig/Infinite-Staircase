using R3;
using System.Linq;

public class MenuInputHandlerPanelCloseButtonViewMediator : Mediator
{
    private readonly MenuInputHandler _menuInputHandler;
    private readonly PanelCloseButtonView[] _panelCloseButtonViews;

    public MenuInputHandlerPanelCloseButtonViewMediator(MenuInputHandler menuInputHandler, 
        PanelCloseButtonView[] panelCloseButtonViews)
    {
        _menuInputHandler = menuInputHandler;
        _panelCloseButtonViews = panelCloseButtonViews;
    }

    public override void Initialize()
    {
        _menuInputHandler.CloseCurrentPressed
            .Where(value => value)
            .Subscribe(_ =>
            {
                PanelCloseButtonView enabledButtonView = _panelCloseButtonViews
                    .FirstOrDefault(b => b.isActiveAndEnabled);

                if (enabledButtonView != null)
                    enabledButtonView.Change();
            })
            .AddTo(CompositeDisposable);
    }
}
