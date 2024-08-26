using R3;
using System;
using UnityEngine.InputSystem;
using Zenject;

public class WindowInputHandler : IInitializable, IDisposable
{
    private readonly PlayerInput _playerInput = new();
    private readonly ReactiveProperty<bool> _closeCurrentPressed = new(false);
    private readonly ReactiveProperty<bool> _pauseMenuSwitchPressed = new(false);
    private readonly ReactiveProperty<bool> _inventorySwitchPressed = new(false);

    public ReadOnlyReactiveProperty<bool> CloseCurrentPressed => _closeCurrentPressed;
    public ReadOnlyReactiveProperty<bool> PauseMenuSwitchPressed => _pauseMenuSwitchPressed;
    public ReadOnlyReactiveProperty<bool> InventorySwitchPressed => _inventorySwitchPressed;

    public void Initialize()
    {
        _playerInput.Windows.CloseCurrent.performed += OnCloseCurrent;
        _playerInput.Windows.CloseCurrent.canceled += OnCloseCurrent;
        _playerInput.Windows.PauseMenuSwitch.performed += OnPauseMenuSwitch;
        _playerInput.Windows.PauseMenuSwitch.canceled += OnPauseMenuSwitch;
        _playerInput.Windows.InventorySwitch.performed += OnInventorySwitch;
        _playerInput.Windows.InventorySwitch.canceled += OnInventorySwitch;

        _playerInput.Enable();
    }

    public void Dispose() => _playerInput.Disable();

    private void OnCloseCurrent(InputAction.CallbackContext context) => 
        _closeCurrentPressed.Value = context.ReadValueAsButton();

    private void OnPauseMenuSwitch(InputAction.CallbackContext context) =>
        _pauseMenuSwitchPressed.Value = context.ReadValueAsButton();

    private void OnInventorySwitch(InputAction.CallbackContext context) =>
        _inventorySwitchPressed.Value = context.ReadValueAsButton();
}
