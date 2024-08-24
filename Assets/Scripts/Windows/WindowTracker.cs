using R3;
using UnityEngine;
using Zenject;

public class WindowTracker : IInitializable
{
    public ReactiveProperty<bool> IsOpen { get; private set; } = new(false);

    public void Initialize() => SetCursor(false);

    public bool TryOpenWindow(GameObject window)
    {
        if (IsOpen.Value)
            return false;

        IsOpen.Value = true;
        SetCursor(true);

        return true;
    }

    public bool TryCloseWindow()
    {
        if (!IsOpen.Value)
            return false;

        IsOpen.Value = false;
        SetCursor(false);

        return true;
    }

    private void SetCursor(bool show)
    {
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }
}
