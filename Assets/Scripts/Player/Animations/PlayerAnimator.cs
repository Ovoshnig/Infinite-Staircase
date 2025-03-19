using R3;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private static readonly int s_isWalkingId = Animator.StringToHash(AnimatorConstants.IsWalking);
    private static readonly int s_isRunningId = Animator.StringToHash(AnimatorConstants.IsRunning);
    private static readonly int s_isJumpingId = Animator.StringToHash(AnimatorConstants.IsJumping);
    private static readonly int s_isGroundedId = Animator.StringToHash(AnimatorConstants.IsGrounded);
    private readonly CompositeDisposable _compositeDisposable = new();
    private PlayerState _playerState;
    private Animator _animator;

    [Inject]
    public void Construct(PlayerState playerState) => _playerState = playerState;

    private void Awake() => _animator = GetComponent<Animator>();

    private void Start()
    {
        _playerState.IsWalking
            .Subscribe(value => _animator.SetBool(s_isWalkingId, value))
            .AddTo(_compositeDisposable);

        _playerState.IsRunning
            .Subscribe(value => _animator.SetBool(s_isRunningId, value))
            .AddTo(_compositeDisposable);

        _playerState.IsJumping
            .Where(value => value)
            .Subscribe(_ => _animator.SetTrigger(s_isJumpingId))
            .AddTo(_compositeDisposable);

        _playerState.IsGrounded
            .Subscribe(value => _animator.SetBool(s_isGroundedId, value))
            .AddTo(_compositeDisposable);
    }

    private void OnDestroy() => _compositeDisposable?.Dispose();
}
