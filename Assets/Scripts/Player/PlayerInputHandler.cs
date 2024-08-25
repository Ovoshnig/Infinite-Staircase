using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerInputHandler : IInitializable, IDisposable
{
    private readonly PlayerInput _playerInput = new();
    private readonly WindowTracker _windowTracker;
    private readonly ReactiveProperty<bool> _isWalkPressed = new(false);
    private readonly ReactiveProperty<bool> _isRunPressed = new(false);
    private readonly ReactiveProperty<bool> _isLookPressed = new(false);
    private readonly ReactiveProperty<bool> _isJumpPressed = new(false);
    private readonly ReactiveProperty<bool> _isTogglePerspectivePressed = new(false);
    private IDisposable _disposable;

    [Inject]
    public PlayerInputHandler(WindowTracker windowTracker) => _windowTracker = windowTracker;

    public Vector2 WalkInput { get; private set; } = Vector2.zero;
    public Vector2 LookInput { get; private set; } = Vector2.zero;
    public ReadOnlyReactiveProperty<bool> IsWalkPressed => _isWalkPressed;
    public ReadOnlyReactiveProperty<bool> IsRunPressed => _isRunPressed;
    public ReadOnlyReactiveProperty<bool> IsLookPressed => _isLookPressed;
    public ReadOnlyReactiveProperty<bool> IsJumpPressed => _isJumpPressed;
    public ReadOnlyReactiveProperty<bool> IsTogglePerspectivePressed => _isTogglePerspectivePressed;

    public void Initialize()
    {
        _disposable = _windowTracker.IsOpen
            .Subscribe(value =>
            {
                if (value)
                    _playerInput.Disable();
                else
                    _playerInput.Enable();
            });

        _playerInput.Player.Walk.performed += OnWalk;
        _playerInput.Player.Walk.canceled += OnWalk;
        _playerInput.Player.Run.performed += OnRun;
        _playerInput.Player.Run.canceled += OnRun;
        _playerInput.Player.Look.performed += OnLook;
        _playerInput.Player.Look.canceled += OnLook;
        _playerInput.Player.Jump.performed += OnJump;
        _playerInput.Player.Jump.canceled += OnJump;
        _playerInput.Player.TogglePerspective.performed += OnTogglePerspective;
        _playerInput.Player.TogglePerspective.canceled += OnTogglePerspective;
    }

    public void Dispose()
    {
        _disposable?.Dispose();

        _playerInput.Disable();
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
