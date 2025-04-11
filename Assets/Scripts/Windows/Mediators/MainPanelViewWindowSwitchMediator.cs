using R3;
using System;
using VContainer.Unity;

public class MainPanelViewWindowSwitchMediator : IInitializable, IDisposable
{
    private readonly MainPanelView _mainPanelView;
    private readonly WindowSwitch _windowSwitch;
    private readonly CompositeDisposable _compositeDisposable = new();

    public MainPanelViewWindowSwitchMediator(MainPanelView mainPanelView, WindowSwitch windowSwitch)
    {
        _mainPanelView = mainPanelView;
        _windowSwitch = windowSwitch;
    }

    public void Initialize()
    {
        Observable
            .EveryValueChanged(this, _ => _mainPanelView.isActiveAndEnabled)
            .Subscribe(_windowSwitch.SetMainPanelActive)
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
