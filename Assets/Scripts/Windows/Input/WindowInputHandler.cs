using R3;
using System;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class WindowInputHandler : IInitializable, IDisposable
{
    private readonly ReactiveProperty<bool> _closeCurrentPressed = new();
    private readonly ReactiveProperty<bool> _pauseMenuSwitchPressed = new();
    private readonly ReactiveProperty<bool> _inventorySwitchPressed = new();
    private InputActionMap _actionMap;

    public ReadOnlyReactiveProperty<bool> CloseCurrentPressed => _closeCurrentPressed;
    public ReadOnlyReactiveProperty<bool> PauseMenuSwitchPressed => _pauseMenuSwitchPressed;
    public ReadOnlyReactiveProperty<bool> InventorySwitchPressed => _inventorySwitchPressed;

    public void Initialize()
    {
        PlayerInput playerInput = new();
        PlayerInput.WindowsActions windowActions = playerInput.Windows;
        _actionMap = InputSystem.actions.FindActionMap(nameof(playerInput.Windows));

        InputAction closeCurrentAction = _actionMap.FindAction(nameof(windowActions.CloseCurrent));
        InputAction switchPauseMenuAction = _actionMap.FindAction(nameof(windowActions.SwitchPauseMenu));
        InputAction switchInventoryAction = _actionMap.FindAction(nameof(windowActions.SwitchInventory));

        closeCurrentAction.performed += OnCloseCurrent;
        closeCurrentAction.canceled += OnCloseCurrent;
        switchPauseMenuAction.performed += OnPauseMenuSwitch;
        switchPauseMenuAction.canceled += OnPauseMenuSwitch;
        switchInventoryAction.performed += OnInventorySwitch;
        switchInventoryAction.canceled += OnInventorySwitch;

        _actionMap.Enable();
    }

    public void Dispose() => _actionMap.Dispose();

    private void OnCloseCurrent(InputAction.CallbackContext context) => 
        _closeCurrentPressed.OnNext(context.ReadValueAsButton());

    private void OnPauseMenuSwitch(InputAction.CallbackContext context) => 
        _pauseMenuSwitchPressed.OnNext(context.ReadValueAsButton());

    private void OnInventorySwitch(InputAction.CallbackContext context) =>
        _inventorySwitchPressed.OnNext(context.ReadValueAsButton());
}
