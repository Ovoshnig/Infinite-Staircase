using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerInputHandler : IInitializable, IDisposable
{
    private readonly WindowTracker _windowTracker;
    private readonly PlayerInput _playerInput = new();

    public event Action WalkPerformed;
    public event Action RunPerformed;
    public event Action WalkCanceled;
    public event Action RunCanceled;
    public event Action JumpPerformed;
    public event Action WindowClosed;

    public Vector2 WalkInput { get; private set; } = Vector2.zero;
    public Vector2 LookInput { get; private set; } = Vector2.zero;
    public bool IsWalkPressed { get; private set; } = false;
    public bool IsLookPressed { get; private set; } = false;
    public bool IsJumpPressed { get; private set; } = false;
    public bool IsRunPressed { get; private set; } = false;

    [Inject]
    public PlayerInputHandler(WindowTracker windowTracker) => _windowTracker = windowTracker;

    public void Initialize()
    {
        _windowTracker.WindowOpened += OnWindowOpened;
        _windowTracker.WindowClosed += OnWindowClosed;

        _playerInput.Player.Walk.performed += OnWalkPerformed;
        _playerInput.Player.Look.performed += OnLookPerformed;
        _playerInput.Player.Jump.performed += OnJumpPerformed;
        _playerInput.Player.Run.performed += OnRunPerformed;
        _playerInput.Player.Walk.canceled += OnWalkCanceled;
        _playerInput.Player.Look.canceled += OnLookCanceled;
        _playerInput.Player.Jump.canceled += OnJumpCanceled;
        _playerInput.Player.Run.canceled += OnRunCanceled;
        _playerInput.Enable();
    }

    public void Dispose() => _playerInput.Disable();

    private void OnWindowOpened() => _playerInput.Disable();

    private void OnWindowClosed()
    {
        _playerInput.Enable();
        WindowClosed?.Invoke();
    }

    private void OnWalkPerformed(InputAction.CallbackContext context)
    {
        WalkInput = context.ReadValue<Vector2>();
        IsWalkPressed = true;
        WalkPerformed?.Invoke();
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
        IsLookPressed = true;
    }

    private void OnJumpPerformed(InputAction.CallbackContext _)
    {
        IsJumpPressed = true;
        JumpPerformed?.Invoke();
    }

    private void OnRunPerformed(InputAction.CallbackContext _)
    {
        IsRunPressed = true;
        RunPerformed?.Invoke();
    }

    private void OnWalkCanceled(InputAction.CallbackContext _)
    {
        WalkInput = Vector2.zero;
        IsWalkPressed = false;
        WalkCanceled?.Invoke();
    }

    private void OnLookCanceled(InputAction.CallbackContext _)
    {
        LookInput = Vector2.zero;
        IsLookPressed = false;
    }

    private void OnJumpCanceled(InputAction.CallbackContext _) => IsJumpPressed = false;

    private void OnRunCanceled(InputAction.CallbackContext _)
    {
        IsRunPressed = false;
        RunCanceled?.Invoke();
    }
}
