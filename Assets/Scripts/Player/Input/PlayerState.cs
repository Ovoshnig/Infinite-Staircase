using R3;
using System;
using UnityEngine;
using Zenject;

public class PlayerState : IInitializable, IDisposable
{
    private readonly PlayerInputHandler _inputHandler;
    private readonly ReactiveProperty<bool> _isWalking = new(false);
    private readonly ReactiveProperty<bool> _isRunning = new(false);
    private readonly ReactiveProperty<bool> _isJumping = new(false);
    private readonly ReactiveProperty<bool> _isLooking = new(false);
    private readonly ReactiveProperty<bool> _isGrounded = new(false);
    private CharacterController _characterController;
    private CompositeDisposable _compositeDisposable;

    [Inject]
    public PlayerState(PlayerInputHandler inputHandler) => _inputHandler = inputHandler;

    public Vector2 WalkInput => _inputHandler.WalkInput;
    public Vector2 LookInput => _inputHandler.LookInput;
    public ReadOnlyReactiveProperty<bool> IsWalking => _isWalking;
    public ReadOnlyReactiveProperty<bool> IsRunning => _isRunning;
    public ReadOnlyReactiveProperty<bool> IsJumping => _isJumping;
    public ReadOnlyReactiveProperty<bool> IsLooking => _isLooking;
    public ReadOnlyReactiveProperty<bool> IsGrounded => _isGrounded;

    public void Initialize()
    {
        _characterController = UnityEngine.Object.FindFirstObjectByType<CharacterController>();

        var walkDisposable = _inputHandler.IsWalkPressed
            .Subscribe(value =>
            {
                _isWalking.Value = value;
                _isRunning.Value = value && _inputHandler.IsRunPressed.CurrentValue;
            });

        var runDisposable = _inputHandler.IsRunPressed
            .Subscribe(value => _isRunning.Value = value && _isWalking.Value);

        var jumpDisposable = _inputHandler.IsJumpPressed
            .Subscribe(value => _isJumping.Value = value && _characterController.isGrounded);

        var lookDisposable = _inputHandler.IsLookPressed
            .Subscribe(value => _isLooking.Value = value);

        var groundDisposable = Observable.EveryUpdate()
            .Select(_ => _characterController.isGrounded)
            .DistinctUntilChanged()
            .Subscribe(value => _isGrounded.Value = value);

        var groundEnterDisposable = _isGrounded
            .Where(value => value)
            .Subscribe(_ => _isJumping.Value = false);

        _compositeDisposable = new CompositeDisposable
        {
            walkDisposable,
            runDisposable,
            lookDisposable,
            jumpDisposable,
            groundDisposable,
            groundEnterDisposable
        };
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
