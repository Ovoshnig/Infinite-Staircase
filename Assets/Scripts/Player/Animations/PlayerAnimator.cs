using R3;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
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
            .Subscribe(value => _animator.SetBool(AnimatorConstants.IsWalking, value))
            .AddTo(_compositeDisposable);

        _playerState.IsRunning
            .Subscribe(value => _animator.SetBool(AnimatorConstants.IsRunning, value))
            .AddTo(_compositeDisposable);

        _playerState.IsJumping
            .Where(value => value)
            .Subscribe(_ => _animator.SetTrigger(AnimatorConstants.IsJumping))
            .AddTo(_compositeDisposable);

        _playerState.IsGrounded
            .Subscribe(value => _animator.SetBool(AnimatorConstants.IsGrounded, value))
            .AddTo(_compositeDisposable);
    }

    private void OnDestroy() => _compositeDisposable?.Dispose();
}
