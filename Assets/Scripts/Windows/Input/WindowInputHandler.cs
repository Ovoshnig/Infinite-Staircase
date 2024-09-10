using R3;
using System;
using UnityEngine.InputSystem;
using Zenject;

public class WindowInputHandler : IInitializable, IDisposable
{
    private readonly PlayerInput _playerInput;
    private readonly Subject<bool> _closeCurrentPressed = new();
    private readonly Subject<bool> _pauseMenuSwitchPressed = new();
    private readonly Subject<bool> _inventorySwitchPressed = new();

    [Inject]
    public WindowInputHandler(PlayerInput playerInput) => _playerInput = playerInput;

    public Observable<bool> CloseCurrentPressed => _closeCurrentPressed;
    public Observable<bool> PauseMenuSwitchPressed => _pauseMenuSwitchPressed;
    public Observable<bool> InventorySwitchPressed => _inventorySwitchPressed;

    public void Initialize()
    {
        _playerInput.Windows.CloseCurrent.performed += OnCloseCurrent;
        _playerInput.Windows.CloseCurrent.canceled += OnCloseCurrent;
        _playerInput.Windows.SwitchPauseMenu.performed += OnPauseMenuSwitch;
        _playerInput.Windows.SwitchPauseMenu.canceled += OnPauseMenuSwitch;
        _playerInput.Windows.SwitchInventory.performed += OnInventorySwitch;
        _playerInput.Windows.SwitchInventory.canceled += OnInventorySwitch;

        _playerInput.Windows.Enable();
    }

    public void Dispose() => _playerInput.Windows.Disable();

    private void OnCloseCurrent(InputAction.CallbackContext context) => 
        _closeCurrentPressed.OnNext(context.ReadValueAsButton());

    private void OnPauseMenuSwitch(InputAction.CallbackContext context) =>
        _pauseMenuSwitchPressed.OnNext(context.ReadValueAsButton());

    private void OnInventorySwitch(InputAction.CallbackContext context) =>
        _inventorySwitchPressed.OnNext(context.ReadValueAsButton());
}
