using R3;
using System;
using VContainer.Unity;

public abstract class WindowSwitch : IWindowSwitch, IInitializable, IDisposable
{
    private readonly WindowInputHandler _windowInputHandler;
    private readonly WindowTracker _windowTracker;
    private readonly ReactiveProperty<bool> _isOpen = new(false);
    private readonly CompositeDisposable _compositeDisposable = new();

    public WindowSwitch(WindowInputHandler windowInputHandler, WindowTracker windowTracker)
    {
        _windowInputHandler = windowInputHandler;
        _windowTracker = windowTracker;
    }

    public ReadOnlyReactiveProperty<bool> IsOpen => _isOpen;

    protected abstract ReadOnlyReactiveProperty<bool> WindowSwitchPressed { get; }
    protected WindowInputHandler WindowInputHandler => _windowInputHandler;

    public virtual void Initialize() 
    { 
        WindowSwitchPressed
            .Where(value => value)
            .Subscribe(_ => OnWindowSwitchPressed())
            .AddTo(_compositeDisposable);
    }

    public virtual void Dispose() => _compositeDisposable?.Dispose();

    protected virtual void OnWindowSwitchPressed()
    {
        if (IsOpen.CurrentValue)
            TryClose();
        else
            TryOpen();
    }

    public virtual bool TryOpen()
    {
        if (!_windowTracker.TryOpenWindow(this, GetType()))
            return false;

        _isOpen.Value = true;

        return true;
    }

    public virtual bool TryClose()
    {
        if (!_windowTracker.TryCloseWindow())
            return false;

        _isOpen.Value = false;

        return true;
    }
}
