using R3;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private static readonly int s_isWalkingHash = Animator.StringToHash(AnimatorConstants.IsWalking);
    private static readonly int s_isRunningHash = Animator.StringToHash(AnimatorConstants.IsRunning);
    private static readonly int s_isJumpingHash = Animator.StringToHash(AnimatorConstants.IsJumping);
    private static readonly int s_isGroundedHash = Animator.StringToHash(AnimatorConstants.IsGrounded);
    private readonly CompositeDisposable _compositeDisposable = new();
    private PlayerState _playerState;
    private Animator _animator;

    [Inject]
    private void Construct([Inject(Id = ZenjectIdConstants.PlayerId)] PlayerState playerState) => 
        _playerState = playerState;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _playerState.IsWalking
            .Subscribe(value => _animator.SetBool(s_isWalkingHash, value))
            .AddTo(_compositeDisposable);

        _playerState.IsRunning
            .Subscribe(value => _animator.SetBool(s_isRunningHash, value))
            .AddTo(_compositeDisposable);

        _playerState.IsJumping
            .Where(value => value)
            .Subscribe(_ => _animator.SetTrigger(s_isJumpingHash))
            .AddTo(_compositeDisposable);

        _playerState.IsGrounded
            .Subscribe(value => _animator.SetBool(s_isGroundedHash, value))
            .AddTo(_compositeDisposable);
    }

    private void OnDestroy() => _compositeDisposable?.Dispose();
}
