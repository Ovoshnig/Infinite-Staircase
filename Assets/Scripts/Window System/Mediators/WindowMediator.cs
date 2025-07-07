using R3;

public class WindowMediator : Mediator
{
    private readonly Window _window;
    private readonly WindowView _windowView;

    public WindowMediator(Window window, WindowView windowView)
    {
        _window = window;
        _windowView = windowView;
    }

    public override void Initialize()
    {
        _window.IsOpen
            .Subscribe(_windowView.gameObject.SetActive)
            .AddTo(CompositeDisposable);

        Observable
            .EveryValueChanged(_windowView, w => w.isActiveAndEnabled)
            .Subscribe(_window.SetWindowActive)
            .AddTo(CompositeDisposable);
    }
}
