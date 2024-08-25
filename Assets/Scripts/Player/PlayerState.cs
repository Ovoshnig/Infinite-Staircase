using R3;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerState : MonoBehaviour
{
    private PlayerInputHandler _inputHandler;
    private ReactiveProperty<bool> _isWalking;
    private ReactiveProperty<bool> _isRunning;
    private ReactiveProperty<bool> _isJumping;
    private ReactiveProperty<bool> _isLooking;
    private CharacterController _characterController;
    private CompositeDisposable _compositeDisposable;

    [Inject]
    private void Construct(PlayerInputHandler inputHandler) => _inputHandler = inputHandler;

    public Vector2 WalkInput => _inputHandler.WalkInput;
    public Vector2 LookInput => _inputHandler.LookInput;
    public ReadOnlyReactiveProperty<bool> IsWalking => _isWalking;
    public ReadOnlyReactiveProperty<bool> IsRunning => _isRunning;
    public ReadOnlyReactiveProperty<bool> IsJumping => _isJumping;
    public ReadOnlyReactiveProperty<bool> IsLooking => _isLooking;
    public ReadOnlyReactiveProperty<bool> IsGrounded { get; private set; }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _isWalking = new ReactiveProperty<bool>(false);
        _isRunning = new ReactiveProperty<bool>(false);
        _isJumping = new ReactiveProperty<bool>(false);
        _isLooking = new ReactiveProperty<bool>(false);

        var moveDisposable = _inputHandler.IsWalkPressed
            .Subscribe(value =>
            {
                _isWalking.Value = value;
                _isRunning.Value = value && _inputHandler.IsRunPressed.CurrentValue;
            });

        var runDisposable = _inputHandler.IsRunPressed
            .Subscribe(value => _isRunning.Value = value && _isWalking.Value);

        var lookDisposable = _inputHandler.IsLookPressed
            .Subscribe(value => _isLooking.Value = value);

        var jumpDisposable = _inputHandler.IsJumpPressed
            .Subscribe(value => _isJumping.Value = value && _characterController.isGrounded);

        IsGrounded = Observable.EveryUpdate()
            .Select(_ => _characterController != null && _characterController.isGrounded)
            .DistinctUntilChanged()
            .ToReadOnlyReactiveProperty();

        _compositeDisposable = new CompositeDisposable
        {
            moveDisposable,
            runDisposable,
            jumpDisposable,
            lookDisposable
        };
    }

    private void OnDestroy() => _compositeDisposable?.Dispose();
}
