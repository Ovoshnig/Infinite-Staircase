using System;
using UnityEngine;

public class WindowTracker
{
    private bool _isWindowOpened = false;

    public event Action WindowOpened;
    public event Action WindowClosed;

    public bool TryOpenWindow(GameObject window)
    {
        if (_isWindowOpened)
            return false;

        _isWindowOpened = true;
        SetCursor(true);
        WindowOpened?.Invoke();

        return true;
    }

    public void CloseWindow()
    {
        if (!_isWindowOpened)
            return;

        _isWindowOpened = false;
        SetCursor(false);
        WindowClosed?.Invoke();
    }

    private void SetCursor(bool show)
    {
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }
}
