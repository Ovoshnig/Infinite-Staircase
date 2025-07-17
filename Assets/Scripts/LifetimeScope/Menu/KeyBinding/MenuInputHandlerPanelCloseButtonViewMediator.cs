using R3;
using System.Linq;

public class MenuInputHandlerPanelCloseButtonViewMediator : Mediator
{
    private readonly MenuInputHandler _menuInputHandler;
    private readonly PanelCloseButtonView[] _panelCloseButtonViews;
    private readonly KeyListeningTracker _keyListeningTracker;

    private bool _previousListening;

    public MenuInputHandlerPanelCloseButtonViewMediator(MenuInputHandler menuInputHandler,
        PanelCloseButtonView[] panelCloseButtonViews,
        KeyListeningTracker keyListeningTracker)
    {
        _menuInputHandler = menuInputHandler;
        _panelCloseButtonViews = panelCloseButtonViews;
        _keyListeningTracker = keyListeningTracker;
    }

    public override void Initialize()
    {
        _menuInputHandler.CloseCurrentPressed
            .Where(isPressed => isPressed)
            .Subscribe(_ => TryClosePanel())
            .AddTo(CompositeDisposable);

        _keyListeningTracker?.IsListening
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
