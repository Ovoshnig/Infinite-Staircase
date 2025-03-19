using R3;
using System;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class WindowInputHandler : IInitializable, IDisposable
{
    private readonly Subject<bool> _closeCurrentPressed = new();
    private readonly Subject<bool> _pauseMenuSwitchPressed = new();
    private readonly Subject<bool> _inventorySwitchPressed = new();
    private InputActionMap _actionMap;

    public Observable<bool> CloseCurrentPressed => _closeCurrentPressed;
    public Observable<bool> PauseMenuSwitchPressed => _pauseMenuSwitchPressed;
    public Observable<bool> InventorySwitchPressed => _inventorySwitchPressed;

    public void Initialize()
    {
        PlayerInput playerInput = new();
        PlayerInput.WindowsActions windowActions = playerInput.Windows;
        _actionMap = InputSystem.actions.FindActionMap(nameof(playerInput.Windows));

        _actionMap.FindAction(nameof(windowActions.CloseCurrent)).performed += OnCloseCurrent;
        _actionMap.FindAction(nameof(windowActions.CloseCurrent)).canceled += OnCloseCurrent;
        _actionMap.FindAction(nameof(windowActions.SwitchPauseMenu)).performed += OnPauseMenuSwitch;
        _actionMap.FindAction(nameof(windowActions.SwitchPauseMenu)).canceled += OnPauseMenuSwitch;
        _actionMap.FindAction(nameof(windowActions.SwitchInventory)).performed += OnInventorySwitch;
        _actionMap.FindAction(nameof(windowActions.SwitchInventory)).canceled += OnInventorySwitch;

        _actionMap.Enable();
    }

    public void Dispose() => _actionMap.Disable();

    private void OnCloseCurrent(InputAction.CallbackContext context) => 
        _closeCurrentPressed.OnNext(context.ReadValueAsButton());

    private void OnPauseMenuSwitch(InputAction.CallbackContext context) => 
        _pauseMenuSwitchPressed.OnNext(context.ReadValueAsButton());

    private void OnInventorySwitch(InputAction.CallbackContext context) =>
        _inventorySwitchPressed.OnNext(context.ReadValueAsButton());
}
