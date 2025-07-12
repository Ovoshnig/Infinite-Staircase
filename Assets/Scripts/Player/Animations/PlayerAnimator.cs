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

    private PlayerState _playerState;
    private Animator _animator;

    [Inject]
    public void Construct(PlayerState playerState) => _playerState = playerState;

    private void Awake() => _animator = GetComponent<Animator>();

    private void Start()
    {
        _playerState.IsWalking
            .Subscribe(isWalking => _animator.SetBool(s_isWalkingId, isWalking))
            .AddTo(this);

        _playerState.IsRunning
            .Subscribe(isRunning => _animator.SetBool(s_isRunningId, isRunning))
            .AddTo(this);

        _playerState.IsJumping
            .Where(isJumping => isJumping)
            .Subscribe(_ => _animator.SetTrigger(s_isJumpingId))
            .AddTo(this);

        _playerState.IsGrounded
            .Subscribe(isGrounded => _animator.SetBool(s_isGroundedId, isGrounded))
            .AddTo(this);
    }
}
