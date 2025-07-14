using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class PlayerInputHandler : IInitializable, IDisposable
{
    private readonly InputActions.PlayerActions _playerActions;
    private readonly ReactiveProperty<bool> _isWalkPressed = new(false);
    private readonly ReactiveProperty<bool> _isRunPressed = new(false);
    private readonly ReactiveProperty<bool> _isLookPressed = new(false);
    private readonly ReactiveProperty<bool> _isZoomPressed = new(false);
    private readonly ReactiveProperty<bool> _isJumpPressed = new(false);
    private readonly ReactiveProperty<bool> _isTogglePerspectivePressed = new(false);

    public PlayerInputHandler(InputActions inputActions) => _playerActions = inputActions.Player;

    public Vector2 WalkInput { get; private set; } = Vector2.zero;
    public Vector2 LookInput { get; private set; } = Vector2.zero;
    public Vector2 ZoomInput { get; private set; } = Vector2.zero;
    public InputAction LookAction { get; private set; } = null;
    public InputAction ZoomAction { get; private set; } = null;
    public ReadOnlyReactiveProperty<bool> IsWalkPressed => _isWalkPressed;
    public ReadOnlyReactiveProperty<bool> IsRunPressed => _isRunPressed;
    public ReadOnlyReactiveProperty<bool> IsLookPressed => _isLookPressed;
    public ReadOnlyReactiveProperty<bool> IsZoomPressed => _isZoomPressed;
    public ReadOnlyReactiveProperty<bool> IsJumpPressed => _isJumpPressed;
    public ReadOnlyReactiveProperty<bool> IsTogglePerspectivePressed => _isTogglePerspectivePressed;

    public void Initialize()
    {
        _playerActions.Enable();

        _playerActions.Walk.Subscribe(OnWalk);
        _playerActions.Run.Subscribe(OnRun);
        _playerActions.Look.Subscribe(OnLook);
        _playerActions.Zoom.Subscribe(OnZoom);
        _playerActions.Jump.Subscribe(OnJump);
        _playerActions.TogglePerspective.Subscribe(OnTogglePerspective);
    }

    public void Dispose()
    {
        _playerActions.Disable();

        _playerActions.Walk.Unsubscribe(OnWalk);
        _playerActions.Run.Unsubscribe(OnRun);
        _playerActions.Look.Unsubscribe(OnLook);
        _playerActions.Zoom.Unsubscribe(OnZoom);
        _playerActions.Jump.Unsubscribe(OnJump);
        _playerActions.TogglePerspective.Unsubscribe(OnTogglePerspective);
    }

    private void OnWalk(InputAction.CallbackContext context)
    {
        WalkInput = context.ReadValue<Vector2>();
        _isWalkPressed.Value = WalkInput != Vector2.zero;
    }

    private void OnRun(InputAction.CallbackContext context) =>
        _isRunPressed.Value = context.ReadValueAsButton();

    private void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
        _isLookPressed.Value = LookInput != Vector2.zero;
        LookAction = context.action;
    }

    private void OnZoom(InputAction.CallbackContext context)
    {
        ZoomInput = context.ReadValue<Vector2>();
        _isZoomPressed.Value = ZoomInput != Vector2.zero;
        ZoomAction = context.action;
    }

    private void OnJump(InputAction.CallbackContext context) =>
        _isJumpPressed.Value = context.ReadValueAsButton();

    private void OnTogglePerspective(InputAction.CallbackContext context) =>
        _isTogglePerspectivePressed.Value = context.ReadValueAsButton();
}
