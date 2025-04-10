using R3;
using System;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class WindowInputHandler : IInitializable, IDisposable
{
    private readonly InputActions _inputActions;
    private readonly ReactiveProperty<bool> _closeCurrentPressed = new();
    private readonly ReactiveProperty<bool> _pauseMenuSwitchPressed = new();
    private readonly ReactiveProperty<bool> _inventorySwitchPressed = new();
    private InputActions.WindowsActions _windowsActions;

    public WindowInputHandler(InputActions inputActions) => _inputActions = inputActions;

    public ReadOnlyReactiveProperty<bool> CloseCurrentPressed => _closeCurrentPressed;
    public ReadOnlyReactiveProperty<bool> PauseMenuSwitchPressed => _pauseMenuSwitchPressed;
    public ReadOnlyReactiveProperty<bool> InventorySwitchPressed => _inventorySwitchPressed;

    public void Initialize()
    {
        _windowsActions = _inputActions.Windows;
        _windowsActions.Enable();

        _windowsActions.CloseCurrent.Subscribe(OnCloseCurrent);
        _windowsActions.SwitchPauseMenu.Subscribe(OnPauseMenuSwitch);
        _windowsActions.SwitchInventory.Subscribe(OnInventorySwitch);
    }

    public void Dispose()
    {
        _windowsActions.Disable();

        _windowsActions.CloseCurrent.Unsubscribe(OnCloseCurrent);
        _windowsActions.SwitchPauseMenu.Unsubscribe(OnPauseMenuSwitch);
        _windowsActions.SwitchInventory.Unsubscribe(OnInventorySwitch);
    }

    private void OnCloseCurrent(InputAction.CallbackContext context) =>
        _closeCurrentPressed.Value = context.ReadValueAsButton();

    private void OnPauseMenuSwitch(InputAction.CallbackContext context) =>
        _pauseMenuSwitchPressed.Value = context.ReadValueAsButton();

    private void OnInventorySwitch(InputAction.CallbackContext context) =>
        _inventorySwitchPressed.Value = context.ReadValueAsButton();
}
