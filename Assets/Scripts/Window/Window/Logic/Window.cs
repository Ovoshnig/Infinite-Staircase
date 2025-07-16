using R3;
using System;
using VContainer.Unity;

public abstract class Window : IWindow, IInitializable, IDisposable
{
    private readonly WindowInputHandler _windowInputHandler;
    private readonly WindowTracker _windowTracker;
    private readonly ReactiveProperty<bool> _isOpen = new(false);
    private readonly CompositeDisposable _compositeDisposable = new();

    private bool _isWindowActive = false;

    public Window(WindowInputHandler windowInputHandler, WindowTracker windowTracker)
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
            .Where(isPressed => isPressed)
            .Subscribe(_ => OnWindowSwitchPressed())
            .AddTo(_compositeDisposable);
            
        WindowInputHandler.CloseCurrentPressed
            .Where(isPressed => isPressed)
            .Subscribe(_ => TryClose())
            .AddTo(_compositeDisposable);
    }

    public virtual void Dispose() => _compositeDisposable.Dispose();

    public virtual bool TryOpen()
    {
        if (!_windowTracker.TryOpenWindow(this))
            return false;

        _isOpen.Value = true;
        return true;
    }

    public virtual bool TryClose()
    {
        if (!_isWindowActive || !_windowTracker.TryCloseWindow())
            return false;

        _isOpen.Value = false;
        return true;
    }

    public void SetWindowActive(bool value) => _isWindowActive = value;

    protected virtual void OnWindowSwitchPressed()
    {
        if (IsOpen.CurrentValue)
            TryClose();
        else
            TryOpen();
    }
}
