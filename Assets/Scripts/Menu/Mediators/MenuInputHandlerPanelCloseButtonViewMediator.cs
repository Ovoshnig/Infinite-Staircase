using R3;
using System;
using System.Linq;
using VContainer.Unity;

public class MenuInputHandlerPanelCloseButtonViewMediator : IInitializable, IDisposable
{
    private readonly MenuInputHandler _menuInputHandler;
    private readonly PanelCloseButtonView[] _panelCloseButtonViews;
    private readonly CompositeDisposable _compositeDisposable = new();

    public MenuInputHandlerPanelCloseButtonViewMediator(MenuInputHandler menuInputHandler, 
        PanelCloseButtonView[] panelCloseButtonViews)
    {
        _menuInputHandler = menuInputHandler;
        _panelCloseButtonViews = panelCloseButtonViews;
    }

    public void Initialize()
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
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
