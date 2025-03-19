using R3;
using System;
using UnityEngine;
using VContainer.Unity;

public class WindowTracker : IInitializable, IDisposable
{
    private readonly WindowInputHandler _inputHandler;
    private readonly ReactiveProperty<bool> _isOpen = new(false);
    private readonly ReactiveProperty<WindowSwitch> _currentWindow = new(null);
    private readonly CompositeDisposable _compositeDisposable = new();
    private Type _windowSwitchType = null;

    public WindowTracker(WindowInputHandler inputHandler) => _inputHandler = inputHandler;

    public ReadOnlyReactiveProperty<bool> IsOpen => _isOpen;

    public void Initialize()
    {
        _currentWindow
            .Subscribe(value => _isOpen.Value = value != null)
            .AddTo(_compositeDisposable);

        _isOpen
            .Subscribe(value => SetCursor(value))
            .AddTo(_compositeDisposable);

        _inputHandler.CloseCurrentPressed
            .Where(value => value)
            .Subscribe(_ =>
            {
                if (_currentWindow.Value != null && _windowSwitchType != typeof(PauseMenuSwitch))
                    _currentWindow.Value.Close();
            })
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();

    public bool TryOpenWindow(WindowSwitch windowSwitch, Type windowSwitchType)
    {
        if (_isOpen.Value)
            return false;

        _currentWindow.Value = windowSwitch;
        _windowSwitchType = windowSwitchType;

        return true;
    }

    public bool TryCloseWindow()
    {
        if (!_isOpen.Value)
            return false;

        _currentWindow.Value = null;

        return true;
    }

    private void SetCursor(bool show)
    {
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }
}
