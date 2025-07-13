using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorView : MonoBehaviour
{
    private static readonly int s_isWalkingId = Animator.StringToHash(AnimatorConstants.IsWalking);
    private static readonly int s_isRunningId = Animator.StringToHash(AnimatorConstants.IsRunning);
    private static readonly int s_isJumpingId = Animator.StringToHash(AnimatorConstants.IsJumping);
    private static readonly int s_isGroundedId = Animator.StringToHash(AnimatorConstants.IsGrounded);

    private Animator _animator;

    private Animator Animator
    {
        get
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();

            return _animator;
        }
    }

    public void SetWalking(bool value) => Animator.SetBool(s_isWalkingId, value);

    public void SetRunning(bool value) => Animator.SetBool(s_isRunningId, value);

    public void SetJumping() => Animator.SetTrigger(s_isJumpingId);

    public void SetGrounded(bool value) => Animator.SetBool(s_isGroundedId, value);
}
