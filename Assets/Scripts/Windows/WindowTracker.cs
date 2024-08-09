using System;
using UnityEngine;
using Zenject;

public class WindowTracker : IInitializable
{
    private readonly GameObject _playerScope;
    private GameObject _openedWindow = null;
    private bool _opened = false;

    public event Action WindowOpened;
    public event Action WindowClosed;

    [Inject]
    public WindowTracker([Inject(Id = BindingConstants.PlayerScopeId)] GameObject playerScope) => 
        _playerScope = playerScope;

    public void Initialize() => SetCursor(false);

    public bool TryOpenWindow(GameObject window)
    {
        if (_opened)
            return false;

        _openedWindow = window;
        _openedWindow.SetActive(true);
        _playerScope.SetActive(false);
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
        _playerScope.SetActive(true);
        _openedWindow = null;
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
