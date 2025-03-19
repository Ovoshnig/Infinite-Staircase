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
    private PlayerInputHandler _inputHandler;
    private CharacterController _characterController;

    [Inject]
    public void Construct(PlayerInputHandler inputHandler, CharacterController characterController)
    {
        _inputHandler = inputHandler;
        _characterController = characterController;
    }

    public Vector2 WalkInput => _inputHandler.WalkInput;
    public Vector2 LookInput => _inputHandler.LookInput;
    public Vector3 EulerAngels => _characterController.transform.eulerAngles;
    public ReadOnlyReactiveProperty<bool> IsWalking => _isWalking;
    public ReadOnlyReactiveProperty<bool> IsRunning => _isRunning;
    public ReadOnlyReactiveProperty<bool> IsGrounded => _isGrounded;
    public ReadOnlyReactiveProperty<bool> IsJumping => _isJumping;
    public ReadOnlyReactiveProperty<bool> IsLooking => _isLooking;

    public void Initialize()
    {
        _inputHandler.IsWalkPressed
           .Subscribe(value =>
           {
               _isWalking.OnNext(value);
               _isRunning.OnNext(value && _inputHandler.IsRunPressed.CurrentValue);
           })
           .AddTo(_compositeDisposable);

        _inputHandler.IsRunPressed
            .Subscribe(value => _isRunning.OnNext(value && IsWalking.CurrentValue))
            .AddTo(_compositeDisposable);

        _inputHandler.IsJumpPressed
           .Subscribe(value => _isJumping.OnNext(value && _characterController.isGrounded))
           .AddTo(_compositeDisposable);

        _inputHandler.IsLookPressed
           .Subscribe(value => _isLooking.OnNext(value))
           .AddTo(_compositeDisposable);

        Observable
           .EveryUpdate()
           .Select(_ => _characterController.isGrounded)
           .DistinctUntilChanged()
           .Subscribe(value => _isGrounded.OnNext(value))
           .AddTo(_compositeDisposable);

        _isGrounded
            .Where(value => value)
            .Subscribe(_ => _isJumping.OnNext(false))
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
