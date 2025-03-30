using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class PlayerInputHandler : IInitializable, IDisposable
{
    private readonly PlayerInput _playerInput;
    private readonly WindowTracker _windowTracker;
    private readonly ReactiveProperty<bool> _isWalkPressed = new(false);
    private readonly ReactiveProperty<bool> _isRunPressed = new(false);
    private readonly ReactiveProperty<bool> _isLookPressed = new(false);
    private readonly ReactiveProperty<bool> _isJumpPressed = new(false);
    private readonly ReactiveProperty<bool> _isTogglePerspectivePressed = new(false);
    private readonly CompositeDisposable _compositeDisposable = new();
    private PlayerInput.PlayerActions _playerActions;

    public PlayerInputHandler(PlayerInput playerInput, WindowTracker windowTracker)
    {
        _playerInput = playerInput;
        _windowTracker = windowTracker;
    }

    public Vector2 WalkInput { get; private set; } = Vector2.zero;
    public Vector2 LookInput { get; private set; } = Vector2.zero;
    public ReadOnlyReactiveProperty<bool> IsWalkPressed => _isWalkPressed;
    public ReadOnlyReactiveProperty<bool> IsRunPressed => _isRunPressed;
    public ReadOnlyReactiveProperty<bool> IsLookPressed => _isLookPressed;
    public ReadOnlyReactiveProperty<bool> IsJumpPressed => _isJumpPressed;
    public ReadOnlyReactiveProperty<bool> IsTogglePerspectivePressed => _isTogglePerspectivePressed;

    public void Initialize()
    {
        _playerActions = _playerInput.Player;

        _playerActions.Walk.Subscribe(OnWalk);
        _playerActions.Run.Subscribe(OnRun);
        _playerActions.Look.Subscribe(OnLook);
        _playerActions.Jump.Subscribe(OnJump);
        _playerActions.TogglePerspective.Subscribe(OnTogglePerspective);

        _windowTracker.IsOpen
            .Subscribe(value =>
            {
                if (value)
                    _playerActions.Disable();
                else
                    _playerActions.Enable();
            })
            .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _playerActions.Disable();

        _playerActions.Walk.Unsubscribe(OnWalk);
        _playerActions.Run.Unsubscribe(OnRun);
        _playerActions.Look.Unsubscribe(OnLook);
        _playerActions.Jump.Unsubscribe(OnJump);
        _playerActions.TogglePerspective.Unsubscribe(OnTogglePerspective);

        _compositeDisposable?.Dispose();
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
    }

    private void OnJump(InputAction.CallbackContext context) =>
        _isJumpPressed.Value = context.ReadValueAsButton();

    private void OnTogglePerspective(InputAction.CallbackContext context) =>
        _isTogglePerspectivePressed.Value = context.ReadValueAsButton();
}
