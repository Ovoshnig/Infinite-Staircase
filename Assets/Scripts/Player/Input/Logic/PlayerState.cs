using R3;
using System;
using UnityEngine;
using VContainer.Unity;

public class PlayerState : IInitializable, IDisposable
{
    private readonly PlayerInputHandler _playerInputHandler;
    private readonly CharacterController _characterController;
    private readonly ReactiveProperty<bool> _isWalking = new(false);
    private readonly ReactiveProperty<bool> _isRunning = new(false);
    private readonly ReactiveProperty<bool> _isLooking = new(false);
    private readonly ReactiveProperty<bool> _isGrounded = new(false);
    private readonly Subject<Unit> _jumped = new();
    private readonly CompositeDisposable _compositeDisposable = new();

    public PlayerState(PlayerInputHandler playerInputHandler,
        CharacterController characterController)
    {
        _playerInputHandler = playerInputHandler;
        _characterController = characterController;
    }

    public Vector2 WalkInput => _playerInputHandler.WalkInput;
    public Vector2 LookInput => _playerInputHandler.LookInput;
    public Vector3 EulerAngles => _characterController.transform.eulerAngles;
    public ReadOnlyReactiveProperty<bool> IsWalking => _isWalking;
    public ReadOnlyReactiveProperty<bool> IsRunning => _isRunning;
    public ReadOnlyReactiveProperty<bool> IsLooking => _isLooking;
    public ReadOnlyReactiveProperty<bool> IsGrounded => _isGrounded;
    public Observable<Unit> Jumped => _jumped;

    public void Initialize()
    {
        Observable
            .EveryValueChanged(this, p => _characterController.isGrounded)
            .Subscribe(isGrounded => _isGrounded.Value = isGrounded)
            .AddTo(_compositeDisposable);
        _playerInputHandler.IsWalkPressed
            .Subscribe(isPressed => _isWalking.Value = isPressed)
            .AddTo(_compositeDisposable);
        _playerInputHandler.IsWalkPressed
            .CombineLatest(
                _playerInputHandler.IsRunPressed,
                _isGrounded,
                _isRunning,
                (walk, runPressed, grounded, wasRunning) =>
                    walk && runPressed && (grounded || wasRunning)
            )
            .Subscribe(shouldRun => _isRunning.Value = shouldRun)
            .AddTo(_compositeDisposable);
        _playerInputHandler.IsLookPressed
            .Subscribe(isPressed => _isLooking.Value = isPressed)
            .AddTo(_compositeDisposable);
        _playerInputHandler.IsJumpPressed
            .Where(isPressed => isPressed && _isGrounded.Value)
            .Subscribe(_ => _jumped.OnNext(Unit.Default))
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable.Dispose();
}
