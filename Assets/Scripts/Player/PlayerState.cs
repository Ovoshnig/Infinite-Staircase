using R3;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerState : MonoBehaviour
{
    private PlayerInputHandler _inputHandler;
    private CharacterController _characterController;
    private CompositeDisposable _compositeDisposable;

    [Inject]
    private void Construct(PlayerInputHandler inputHandler) => _inputHandler = inputHandler;

    public Vector2 WalkInput => _inputHandler.WalkInput;
    public Vector2 LookInput => _inputHandler.LookInput;
    public ReactiveProperty<bool> IsWalking { get; private set; } 
    public ReactiveProperty<bool> IsRunning { get; private set; } 
    public ReactiveProperty<bool> IsJumping { get; private set; } 
    public ReactiveProperty<bool> IsLooking { get; private set; } 
    public ReadOnlyReactiveProperty<bool> IsInAir { get; private set; } 

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        IsWalking = new ReactiveProperty<bool>(false);
        IsRunning = new ReactiveProperty<bool>(false);
        IsJumping = new ReactiveProperty<bool>(false);
        IsLooking = new ReactiveProperty<bool>(false);
        IsInAir = new ReactiveProperty<bool>(false);

        var moveDisposable = _inputHandler.IsWalkPressed
            .Subscribe(value =>
            {
                IsWalking.Value = value;
                IsRunning.Value = value && _inputHandler.IsRunPressed.Value;
            });

        var runDisposable = _inputHandler.IsRunPressed
            .Subscribe(value => IsRunning.Value = value && IsWalking.Value);

        var lookDisposable = _inputHandler.IsLookPressed
            .Subscribe(value => IsLooking.Value = value);

        var jumpDisposable = _inputHandler.IsJumpPressed
            .Subscribe(value => IsJumping.Value = value && _characterController.isGrounded);

        IsInAir = Observable.EveryUpdate()
            .Select(_ => _characterController != null && !_characterController.isGrounded)
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
