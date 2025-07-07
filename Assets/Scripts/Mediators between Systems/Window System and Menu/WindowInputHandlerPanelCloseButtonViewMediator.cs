using R3;
using System.Linq;

public class WindowInputHandlerPanelCloseButtonViewMediator : Mediator
{
    private readonly WindowInputHandler _windowInputHandler;
    private readonly PanelCloseButtonView[] _panelCloseButtonViews;

    public WindowInputHandlerPanelCloseButtonViewMediator(WindowInputHandler windowInputHandler, 
        PanelCloseButtonView[] panelCloseButtonViews)
    {
        _windowInputHandler = windowInputHandler;
        _panelCloseButtonViews = panelCloseButtonViews;
    }

    public override void Initialize()
    {
        _windowInputHandler.CloseCurrentPressed
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
