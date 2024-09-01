using R3;
using UnityEngine;

public class ThirdPersonPlayerHorizontalMover : PlayerHorizontalMover
{
    protected override void Awake()
    {
        base.Awake();

        var cameraSwitchDisposable = CameraSwitch.IsFirstPerson
            .Subscribe(value => IsMoving = !value);

        CompositeDisposable.Add(cameraSwitchDisposable);
    }

    protected override void Move()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        if (PlayerState.IsWalking.CurrentValue)
        {
            var currentSpeed = PlayerState.IsRunning.CurrentValue ? RunSpeed : WalkSpeed;
            MoveDirection = forward * currentSpeed;
        }
        else
        {
            MoveDirection = Vector3.zero;
        }

        base.Move();
    }
}
