using R3;
using System;
using UnityEngine.InputSystem;
using Zenject;

public class WindowInputHandler : IInitializable, IDisposable
{
    private readonly PlayerInput _playerInput = new();
    private readonly Subject<bool> _closeCurrentPressed = new();
    private readonly Subject<bool> _pauseMenuSwitchPressed = new();
    private readonly Subject<bool> _inventorySwitchPressed = new();

    public Observable<bool> CloseCurrentPressed => _closeCurrentPressed;
    public Observable<bool> PauseMenuSwitchPressed => _pauseMenuSwitchPressed;
    public Observable<bool> InventorySwitchPressed => _inventorySwitchPressed;

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
        _closeCurrentPressed.OnNext(context.ReadValueAsButton());

    private void OnPauseMenuSwitch(InputAction.CallbackContext context) =>
        _pauseMenuSwitchPressed.OnNext(context.ReadValueAsButton());

    private void OnInventorySwitch(InputAction.CallbackContext context) =>
        _inventorySwitchPressed.OnNext(context.ReadValueAsButton());
}
