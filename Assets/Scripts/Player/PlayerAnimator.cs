using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerState _playerState;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _playerState.WalkStarted += OnWalkStarted;
        _playerState.RunStarted += OnRunStarted;
        _playerState.JumpStarted += OnJumpStarted;
        _playerState.WalkEnded += OnWalkEnded;
        _playerState.RunEnded += OnRunEnded;
        _playerState.JumpEnded += OnJumpEnded;
    }

    private void OnDestroy()
    {
        _playerState.WalkStarted -= OnWalkStarted;
        _playerState.RunStarted -= OnRunStarted;
        _playerState.JumpStarted -= OnJumpStarted;
        _playerState.WalkEnded -= OnWalkEnded;
        _playerState.RunEnded -= OnRunEnded;
        _playerState.JumpEnded -= OnJumpEnded;
    }

    private void OnWalkStarted() => _animator.SetBool(AnimatorConstants.IsWalking, true);

    private void OnRunStarted() => _animator.SetBool(AnimatorConstants.IsRunning, true);

    private void OnJumpStarted() => _animator.SetBool("isJumping", true);

    private void OnWalkEnded() => _animator.SetBool(AnimatorConstants.IsWalking, false);

    private void OnRunEnded() => _animator.SetBool(AnimatorConstants.IsRunning, false);

    private void OnJumpEnded() => _animator.SetBool("isJumping", false);
}
