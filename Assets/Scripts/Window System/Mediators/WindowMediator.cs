using R3;
using System;
using VContainer.Unity;

public class WindowMediator : IInitializable, IDisposable
{
    private readonly Window _window;
    private readonly WindowView _windowView;
    private readonly CompositeDisposable _compositeDisposable = new();

    public WindowMediator(Window window, WindowView windowView)
    {
        _window = window;
        _windowView = windowView;
    }

    public void Initialize()
    {
        _window.IsOpen
            .Subscribe(_windowView.gameObject.SetActive)
            .AddTo(_compositeDisposable);

        Observable
            .EveryValueChanged(_windowView, w => w.isActiveAndEnabled)
            .Subscribe(_window.SetWindowActive)
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
