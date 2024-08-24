using R3;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerState _playerState;

    private Animator _animator;
    private CompositeDisposable _compositeDisposable;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        var walkDisposable = _playerState.IsWalking
            .Subscribe(value => _animator.SetBool(AnimatorConstants.IsWalking, value));

        var runDisposable = _playerState.IsRunning
            .Subscribe(value => _animator.SetBool(AnimatorConstants.IsRunning, value));

        var jumpDisposable = _playerState.IsJumping
            .Subscribe(value => _animator.SetBool(AnimatorConstants.IsJumping, value));

        var inAirDisposable = _playerState.IsInAir
            .Subscribe(value => _animator.SetBool(AnimatorConstants.IsGrounded, !value));

        _compositeDisposable = new CompositeDisposable()
        {
            walkDisposable,
            runDisposable,
            jumpDisposable,
            inAirDisposable
        };
    }

    private void OnDestroy() => _compositeDisposable?.Dispose();
}
