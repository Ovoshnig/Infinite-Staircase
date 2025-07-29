using R3;
using System;
using UnityEngine;
using VContainer.Unity;

public class PlayerState : IInitializable, IDisposable
{
    private readonly PlayerInputHandler _playerInputHandler;
    private readonly CharacterController _characterController;
    private readonly Subject<Unit> _jumped = new();
    private readonly CompositeDisposable _compositeDisposable = new();

    public PlayerState(PlayerInputHandler playerInputHandler,
        CharacterController characterController)
    {
        _playerInputHandler = playerInputHandler;
        _characterController = characterController;
    }

    public ReadOnlyReactiveProperty<Vector2> WalkInput => _playerInputHandler.WalkInput;
    public ReadOnlyReactiveProperty<Vector2> LookInput => _playerInputHandler.LookInput;
    public ReadOnlyReactiveProperty<bool> IsWalking { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsLooking { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsRunning { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsGrounded { get; private set; }
    public Observable<Unit> Jumped => _jumped;
    public Vector3 EulerAngles => _characterController.transform.eulerAngles;

    public void Initialize()
    {
        IsWalking = WalkInput
            .Select(value => value != Vector2.zero)
            .ToReadOnlyReactiveProperty()
            .AddTo(_compositeDisposable);
        IsLooking = LookInput
            .Select(value => value != Vector2.zero)
            .ToReadOnlyReactiveProperty()
            .AddTo(_compositeDisposable);

        IsGrounded = Observable
            .EveryValueChanged(this, p => _characterController.isGrounded)
            .ToReadOnlyReactiveProperty()
            .AddTo(_compositeDisposable);
        IsRunning = IsWalking
            .CombineLatest(
                _playerInputHandler.IsRunPressed,
                (isWalking, isRunning) =>
                    isWalking && isRunning
            )
            .ToReadOnlyReactiveProperty()
            .AddTo(_compositeDisposable);
        _playerInputHandler.IsJumpPressed
            .Where(isPressed => isPressed && IsGrounded.CurrentValue)
            .Subscribe(_ => _jumped.OnNext(Unit.Default))
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable.Dispose();
}
