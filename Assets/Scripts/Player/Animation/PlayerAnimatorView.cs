using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorView : MonoBehaviour
{
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

    public void SetWalking(bool value) => Animator.SetBool(PlayerAnimatorConstants.IsWalkingId, value);

    public void SetRunning(bool value) => Animator.SetBool(PlayerAnimatorConstants.IsRunningId, value);

    public void SetGrounded(bool value) => Animator.SetBool(PlayerAnimatorConstants.IsGroundedId, value);

    public void SetJumped() => Animator.SetTrigger(PlayerAnimatorConstants.JumpedId);
}
