using R3;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[RequireComponent(typeof(CharacterController))]
public class PlayerState : IInitializable, IDisposable
{
    private readonly ReactiveProperty<bool> _isWalking = new(false);
    private readonly ReactiveProperty<bool> _isRunning = new(false);
    private readonly ReactiveProperty<bool> _isJumping = new(false);
    private readonly ReactiveProperty<bool> _isLooking = new(false);
    private readonly ReactiveProperty<bool> _isGrounded = new(false);
    private readonly CompositeDisposable _compositeDisposable = new();
    private PlayerInputHandler _playerInputHandler;
    private CharacterController _characterController;

    [Inject]
    public void Construct(PlayerInputHandler playerInputHandler, CharacterController characterController)
    {
        _playerInputHandler = playerInputHandler;
        _characterController = characterController;
    }

    public Vector2 WalkInput => _playerInputHandler.WalkInput;
    public Vector2 LookInput => _playerInputHandler.LookInput;
    public Vector3 EulerAngels => _characterController.transform.eulerAngles;
    public ReadOnlyReactiveProperty<bool> IsWalking => _isWalking;
    public ReadOnlyReactiveProperty<bool> IsRunning => _isRunning;
    public ReadOnlyReactiveProperty<bool> IsGrounded => _isGrounded;
    public ReadOnlyReactiveProperty<bool> IsJumping => _isJumping;
    public ReadOnlyReactiveProperty<bool> IsLooking => _isLooking;

    public void Initialize()
    {
        _playerInputHandler.IsWalkPressed
           .Subscribe(value =>
           {
               _isWalking.OnNext(value);
               _isRunning.OnNext(value && _playerInputHandler.IsRunPressed.CurrentValue);
           })
           .AddTo(_compositeDisposable);

        _playerInputHandler.IsRunPressed
            .Subscribe(value => _isRunning.OnNext(value && IsWalking.CurrentValue))
            .AddTo(_compositeDisposable);

        _playerInputHandler.IsJumpPressed
           .Subscribe(value => _isJumping.OnNext(value && _characterController.isGrounded))
           .AddTo(_compositeDisposable);

        _playerInputHandler.IsLookPressed
           .Subscribe(value => _isLooking.OnNext(value))
           .AddTo(_compositeDisposable);

        Observable
           .EveryValueChanged(this, c => _characterController.isGrounded)
           .Subscribe(value => _isGrounded.OnNext(value))
           .AddTo(_compositeDisposable);

        _isGrounded
            .Where(value => value)
            .Subscribe(_ => _isJumping.OnNext(false))
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
