using R3;
using System;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class MenuInputHandler : IInitializable, IDisposable
{
    private readonly InputActions _inputActions;
    private readonly ReactiveProperty<bool> _closeCurrentPressed = new();
    private InputActions.MenuActions _menuActions;

    public MenuInputHandler(InputActions inputActions) => _inputActions = inputActions;

    public ReadOnlyReactiveProperty<bool> CloseCurrentPressed => _closeCurrentPressed;

    public void Initialize()
    {
        _menuActions = _inputActions.Menu;
        _menuActions.Enable();

        _menuActions.CloseCurrent.Subscribe(OnCloseCurrent);
    }

    public void Dispose()
    {
        _menuActions.Disable();

        _menuActions.CloseCurrent.Unsubscribe(OnCloseCurrent);
    }

    private void OnCloseCurrent(InputAction.CallbackContext context) =>
        _closeCurrentPressed.Value = context.ReadValueAsButton();
}
