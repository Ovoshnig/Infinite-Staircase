using R3;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private PlayerState _playerState;
    private Animator _animator;
    private CompositeDisposable _compositeDisposable;

    [Inject]
    private void Construct([Inject(Id = ZenjectIdConstants.PlayerId)] PlayerState playerState) => 
        _playerState = playerState;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        var walkDisposable = _playerState.IsWalking
            .Subscribe(value => _animator.SetBool(AnimatorConstants.IsWalking, value));

        var runDisposable = _playerState.IsRunning
            .Subscribe(value => _animator.SetBool(AnimatorConstants.IsRunning, value));

        var jumpDisposable = _playerState.IsJumping
            .Subscribe(value => _animator.SetBool(AnimatorConstants.IsJumping, value));

        var groundDisposable = _playerState.IsGrounded
            .Subscribe(value => _animator.SetBool(AnimatorConstants.IsGrounded, value));

        _compositeDisposable = new CompositeDisposable()
        {
            walkDisposable,
            runDisposable,
            jumpDisposable,
            groundDisposable
        };
    }

    private void OnDestroy() => _compositeDisposable?.Dispose();
}
