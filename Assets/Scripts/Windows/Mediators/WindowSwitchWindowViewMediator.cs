using R3;
using System;
using VContainer.Unity;

public class WindowSwitchWindowViewMediator : IInitializable, IDisposable
{
    private readonly WindowSwitch _windowSwitch;
    private readonly WindowView _windowView;
    private readonly CompositeDisposable _compositeDisposable = new();

    public WindowSwitchWindowViewMediator(WindowSwitch windowSwitch, WindowView windowView)
    {
        _windowSwitch = windowSwitch;
        _windowView = windowView;
    }

    public void Initialize()
    {
        _windowSwitch.IsOpen
            .Subscribe(value =>
            {
                if (value)
                    _windowView.Activate();
                else
                    _windowView.Deactivate();
            })
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
