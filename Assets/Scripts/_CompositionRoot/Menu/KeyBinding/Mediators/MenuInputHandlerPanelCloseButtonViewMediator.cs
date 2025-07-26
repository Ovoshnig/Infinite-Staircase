using R3;
using System.Linq;

public class MenuInputHandlerPanelCloseButtonViewMediator : Mediator
{
    private readonly MenuInputHandler _menuInputHandler;
    private readonly PanelCloseButtonView[] _panelCloseButtonViews;
    private readonly ButtonListener _buttonListener;

    private bool _previousListening;

    public MenuInputHandlerPanelCloseButtonViewMediator(MenuInputHandler menuInputHandler,
        PanelCloseButtonView[] panelCloseButtonViews,
        ButtonListener buttonListener)
    {
        _menuInputHandler = menuInputHandler;
        _panelCloseButtonViews = panelCloseButtonViews;
        _buttonListener = buttonListener;
    }

    public override void Initialize()
    {
        _menuInputHandler.CloseCurrentPressed
            .Where(isPressed => isPressed)
            .Subscribe(_ => TryClosePanel())
            .AddTo(CompositeDisposable);

        _buttonListener?.IsListening
            .DelayFrame(1)
            .Subscribe(isListening => _previousListening = isListening)
            .AddTo(CompositeDisposable);
    }

    private bool TryClosePanel()
    {
        PanelCloseButtonView enabledButtonView = _panelCloseButtonViews
            .FirstOrDefault(b => b.isActiveAndEnabled);

        if (enabledButtonView == null || _previousListening)
            return false;

        enabledButtonView.Switch();
        return true;
    }
}
