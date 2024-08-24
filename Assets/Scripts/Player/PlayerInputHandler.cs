using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerInputHandler : IInitializable, IDisposable
{
    private readonly WindowTracker _windowTracker;
    private readonly PlayerInput _playerInput = new();

    [Inject]
    public PlayerInputHandler(WindowTracker windowTracker) => _windowTracker = windowTracker;

    public Vector2 WalkInput { get; private set; } = Vector2.zero;
    public Vector2 LookInput { get; private set; } = Vector2.zero;
    public ReactiveProperty<bool> IsWalkPressed { get; private set; } = 
        new ReactiveProperty<bool>(false);
    public ReactiveProperty<bool> IsRunPressed { get; private set; } = 
        new ReactiveProperty<bool>(false);
    public ReactiveProperty<bool> IsLookPressed { get; private set; } = 
        new ReactiveProperty<bool>(false);
    public ReactiveProperty<bool> IsJumpPressed { get; private set; } = 
        new ReactiveProperty<bool>(false);
    public ReactiveProperty<bool> IsTogglePerspectivePressed { get; private set; } = 
        new ReactiveProperty<bool>(false);

    public void Initialize()
    {
        _windowTracker.WindowOpened += OnWindowOpened;
        _windowTracker.WindowClosed += OnWindowClosed;

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

        _playerInput.Enable();
    }

    public void Dispose()
    {
        _windowTracker.WindowOpened -= OnWindowOpened;
        _windowTracker.WindowClosed -= OnWindowClosed;

        _playerInput.Disable();
    }

    private void OnWindowOpened() => _playerInput.Disable();

    private void OnWindowClosed() => _playerInput.Enable();

    private void OnWalk(InputAction.CallbackContext context)
    {
        WalkInput = context.ReadValue<Vector2>();
        IsWalkPressed.Value = WalkInput != Vector2.zero;
    }

    private void OnRun(InputAction.CallbackContext context) =>
        IsRunPressed.Value = context.ReadValueAsButton();

    private void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
        IsLookPressed.Value = LookInput != Vector2.zero;
    }

    private void OnJump(InputAction.CallbackContext context) => 
        IsJumpPressed.Value = context.ReadValueAsButton();

    private void OnTogglePerspective(InputAction.CallbackContext context) => 
        IsTogglePerspectivePressed.Value = context.ReadValueAsButton();
}
