using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class PlayerInputHandler : IInitializable, IDisposable
{
    private readonly InputActions.PlayerActions _playerActions;
    private readonly CompositeDisposable _compositeDisposable = new();

    public PlayerInputHandler(InputActions inputActions) => _playerActions = inputActions.Player;

    public InputAction LookAction => _playerActions.Look;
    public InputAction ZoomAction => _playerActions.Zoom;
    public ReadOnlyReactiveProperty<Vector2> WalkInput { get; private set; }
    public ReadOnlyReactiveProperty<Vector2> LookInput { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsRunPressed { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsZoomPressed { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsJumpPressed { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsTogglePerspectivePressed { get; private set; }

    public void Initialize()
    {
        _playerActions.Enable();

        WalkInput = _playerActions.Walk
            .AsValueStream<Vector2>()
            .AddTo(_compositeDisposable);
        LookInput = _playerActions.Look
            .AsValueStream<Vector2>()
            .AddTo(_compositeDisposable);

        IsRunPressed = _playerActions.Run
            .AsButtonStream()
            .AddTo(_compositeDisposable);
        IsZoomPressed = _playerActions.Zoom
            .AsButtonStream()
            .AddTo(_compositeDisposable);
        IsJumpPressed = _playerActions.Jump
            .AsButtonStream()
            .AddTo(_compositeDisposable);
        IsTogglePerspectivePressed = _playerActions.TogglePerspective
            .AsButtonStream()
            .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
        _playerActions.Disable();
    }
}
