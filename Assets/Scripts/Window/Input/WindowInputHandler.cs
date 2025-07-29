using R3;
using System;
using VContainer.Unity;

public class WindowInputHandler : IInitializable, IDisposable
{
    private readonly InputActions.WindowsActions _windowsActions;
    private readonly CompositeDisposable _compositeDisposable = new();

    public WindowInputHandler(InputActions inputActions) => _windowsActions = inputActions.Windows;

    public ReadOnlyReactiveProperty<bool> CloseCurrentPressed { get; private set; }
    public ReadOnlyReactiveProperty<bool> PauseMenuSwitchPressed { get; private set; }
    public ReadOnlyReactiveProperty<bool> InventorySwitchPressed { get; private set; }

    public void Initialize()
    {
        _windowsActions.Enable();

        CloseCurrentPressed = _windowsActions.CloseCurrent
            .AsButtonStream()
            .AddTo(_compositeDisposable);
        PauseMenuSwitchPressed = _windowsActions.SwitchPauseMenu
            .AsButtonStream()
            .AddTo(_compositeDisposable);
        InventorySwitchPressed = _windowsActions.SwitchInventory
            .AsButtonStream()
            .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
        _windowsActions.Disable();
    }
}
