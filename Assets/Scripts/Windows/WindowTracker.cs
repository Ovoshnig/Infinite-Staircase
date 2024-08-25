using R3;
using System;
using UnityEngine;
using Zenject;

public class WindowTracker : IInitializable, IDisposable
{
    private readonly ReactiveProperty<bool> _isOpen = new(false);
    private IDisposable _disposable;

    public ReadOnlyReactiveProperty<bool> IsOpen => _isOpen;

    public void Initialize()
    {
        _disposable = _isOpen
            .Subscribe(value => SetCursor(value));
    }

    public void Dispose() => _disposable?.Dispose();

    public bool TryOpenWindow()
    {
        if (_isOpen.Value)
            return false;

        _isOpen.Value = true;

        return true;
    }

    public bool TryCloseWindow()
    {
        if (!_isOpen.Value)
            return false;

        _isOpen.Value = false;

        return true;
    }

    private void SetCursor(bool show)
    {
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }
}
