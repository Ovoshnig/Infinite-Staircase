using R3;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerState : MonoBehaviour
{
    private readonly ReactiveProperty<bool> _isWalking = new(false);
    private readonly ReactiveProperty<bool> _isRunning = new(false);
    private readonly ReactiveProperty<bool> _isJumping = new(false);
    private readonly ReactiveProperty<bool> _isLooking = new(false);
    private readonly ReactiveProperty<bool> _isGrounded = new(false);
    private PlayerInputHandler _inputHandler;
    private CharacterController _characterController;
    private CompositeDisposable _compositeDisposable;

    [Inject]
    private void Construct(PlayerInputHandler inputHandler) => _inputHandler = inputHandler;

    public Vector2 WalkInput => _inputHandler.WalkInput;
    public Vector2 LookInput => _inputHandler.LookInput;
    public ReadOnlyReactiveProperty<bool> IsWalking => _isWalking;
    public ReadOnlyReactiveProperty<bool> IsRunning => _isRunning;
    public ReadOnlyReactiveProperty<bool> IsGrounded => _isGrounded;
    public ReadOnlyReactiveProperty<bool> IsJumping => _isJumping;
    public ReadOnlyReactiveProperty<bool> IsLooking => _isLooking;

    public void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        var walkDisposable = _inputHandler.IsWalkPressed
            .Subscribe(value =>
            {
                _isWalking.OnNext(value);
                _isRunning.OnNext(value && _inputHandler.IsRunPressed.CurrentValue);
            });

        var runDisposable = _inputHandler.IsRunPressed
            .Subscribe(value => _isRunning.OnNext(value && IsWalking.CurrentValue));

        var jumpDisposable = _inputHandler.IsJumpPressed
            .Subscribe(value => _isJumping.OnNext(value && _characterController.isGrounded));

        var lookDisposable = _inputHandler.IsLookPressed
            .Subscribe(value => _isLooking.OnNext(value));

        var groundDisposable = Observable.EveryUpdate()
            .Select(_ => _characterController != null && _characterController.isGrounded)
            .DistinctUntilChanged()
            .Subscribe(value => _isGrounded.OnNext(value));

        var groundEnterDisposable = _isGrounded
            .Where(value => value)
            .Subscribe(_ => _isJumping.OnNext(false));

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

    public void Destroy() => _compositeDisposable?.Dispose();
}
