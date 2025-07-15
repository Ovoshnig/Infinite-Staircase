using UnityEngine;

public static class PlayerAnimatorConstants
{
    public static readonly int IsWalkingId = Animator.StringToHash("isWalking");
    public static readonly int IsRunningId = Animator.StringToHash("isRunning");
    public static readonly int IsGroundedId = Animator.StringToHash("isGrounded");
    public static readonly int JumpedId = Animator.StringToHash("jumped");
}
