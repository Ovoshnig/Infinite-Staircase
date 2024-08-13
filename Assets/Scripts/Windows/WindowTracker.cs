using System;
using UnityEngine;
using Zenject;

public class WindowTracker : IInitializable
{
    private bool _opened = false;

    public event Action WindowOpened;
    public event Action WindowClosed;

    public void Initialize() => SetCursor(false);

    public bool TryOpenWindow(GameObject window)
    {
        if (_opened)
            return false;

        _opened = true;
        SetCursor(true);
        WindowOpened?.Invoke();

        return true;
    }

    public bool TryCloseWindow()
    {
        if (!_opened)
            return false;

        _opened = false;
        SetCursor(false);
        WindowClosed?.Invoke();

        return true;
    }

    private void SetCursor(bool show)
    {
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }
}
