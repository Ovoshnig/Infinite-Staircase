using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class PlayerInputHandler : IInitializable, IDisposable
{
    private readonly WindowTracker _windowTracker;
    private readonly ReactiveProperty<bool> _isWalkPressed = new(false);
    private readonly ReactiveProperty<bool> _isRunPressed = new(false);
    private readonly ReactiveProperty<bool> _isLookPressed = new(false);
    private readonly ReactiveProperty<bool> _isJumpPressed = new(false);
    private readonly ReactiveProperty<bool> _isTogglePerspectivePressed = new(false);
    private readonly CompositeDisposable _compositeDisposable = new();
    private InputActionMap _actionMap;

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
        PlayerInput playerInput = new();
        PlayerInput.PlayerActions playerActions = playerInput.Player;
        _actionMap = InputSystem.actions.FindActionMap(nameof(playerInput.Player));

        _actionMap.FindAction(nameof(playerActions.Walk)).performed += OnWalk;
        _actionMap.FindAction(nameof(playerActions.Walk)).canceled += OnWalk;
        _actionMap.FindAction(nameof(playerActions.Run)).performed += OnRun;
        _actionMap.FindAction(nameof(playerActions.Run)).canceled += OnRun;
        _actionMap.FindAction(nameof(playerActions.Look)).performed += OnLook;
        _actionMap.FindAction(nameof(playerActions.Look)).canceled += OnLook;
        _actionMap.FindAction(nameof(playerActions.Jump)).performed += OnJump;
        _actionMap.FindAction(nameof(playerActions.Jump)).canceled += OnJump;
        _actionMap.FindAction(nameof(playerActions.TogglePerspective)).performed += OnTogglePerspective;
        _actionMap.FindAction(nameof(playerActions.TogglePerspective)).canceled += OnTogglePerspective;

        _windowTracker.IsOpen
            .Subscribe(value =>
            {
                if (value)
                    _actionMap.Disable();
                else
                    _actionMap.Enable();
            })
            .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _actionMap.Disable();

        _compositeDisposable?.Dispose();
    }

    private void OnWalk(InputAction.CallbackContext context)
    {
        WalkInput = context.ReadValue<Vector2>();
        _isWalkPressed.OnNext(WalkInput != Vector2.zero);
    }

    private void OnRun(InputAction.CallbackContext context) =>
        _isRunPressed.OnNext(context.ReadValueAsButton());

    private void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
        _isLookPressed.OnNext(LookInput != Vector2.zero);
    }

    private void OnJump(InputAction.CallbackContext context) => 
        _isJumpPressed.OnNext(context.ReadValueAsButton());

    private void OnTogglePerspective(InputAction.CallbackContext context) => 
        _isTogglePerspectivePressed.OnNext(context.ReadValueAsButton());
}
