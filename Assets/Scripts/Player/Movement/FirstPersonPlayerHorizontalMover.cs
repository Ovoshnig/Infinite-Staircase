using R3;
using UnityEngine;

public class FirstPersonPlayerHorizontalMover : PlayerHorizontalMover
{
    protected override void Awake()
    {
        base.Awake();

        var cameraSwitchDisposable = CameraSwitch.IsFirstPerson
            .Subscribe(value => IsMoving = value);

        CompositeDisposable.Add(cameraSwitchDisposable);
    }

    protected override void Move()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        var currentSpeedX = PlayerState.WalkInput.y;
        currentSpeedX *= PlayerState.IsRunning.CurrentValue ? RunSpeed : WalkSpeed;
        var currentSpeedZ = PlayerState.WalkInput.x;
        currentSpeedZ *= PlayerState.IsRunning.CurrentValue ? RunSpeed : WalkSpeed;

        MoveDirection = (forward * currentSpeedX) + (right * currentSpeedZ);

        base.Move();
    }
}
