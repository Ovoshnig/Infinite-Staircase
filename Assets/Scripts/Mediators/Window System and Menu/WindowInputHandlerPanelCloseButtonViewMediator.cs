using R3;
using System;
using System.Linq;
using VContainer.Unity;

public class WindowInputHandlerPanelCloseButtonViewMediator : IInitializable, IDisposable
{
    private readonly WindowInputHandler _windowInputHandler;
    private readonly PanelCloseButtonView[] _panelCloseButtonViews;
    private readonly CompositeDisposable _compositeDisposable = new();

    public WindowInputHandlerPanelCloseButtonViewMediator(WindowInputHandler windowInputHandler, 
        PanelCloseButtonView[] panelCloseButtonViews)
    {
        _windowInputHandler = windowInputHandler;
        _panelCloseButtonViews = panelCloseButtonViews;
    }

    public void Initialize()
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
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
