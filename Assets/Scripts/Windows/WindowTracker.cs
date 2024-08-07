using System;
using UnityEngine;

public class WindowTracker
{
    private GameObject _openedWindow = null;
    private bool _opened = false;

    public event Action WindowOpened;
    public event Action WindowClosed;

    public bool TryOpenWindow(GameObject window)
    {
        if (_opened)
            return false;

        _openedWindow = window;
        _openedWindow.SetActive(true);
        _opened = true;
        SetCursor(true);
        WindowOpened?.Invoke();

        return true;
    }

    public bool TryCloseWindow()
    {
        if (!_opened)
            return false;

        _openedWindow.SetActive(false);
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
