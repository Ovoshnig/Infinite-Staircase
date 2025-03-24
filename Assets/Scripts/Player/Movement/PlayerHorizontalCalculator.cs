using UnityEngine;

public class PlayerHorizontalCalculator
{
    private readonly PlayerSettings _playerSettings;
    private readonly PlayerState _playerState;

    public PlayerHorizontalCalculator(PlayerSettings playerSettings, PlayerState playerState)
    {
        _playerSettings = playerSettings;
        _playerState = playerState;
    }

    public Vector3 CalculateHorizontalVector(ref Vector3 movement, Vector2 walkInput, 
        float playerAngleY, float cameraAngleY)
    {
        Vector3 eulerAngles;

        if (_playerState.IsWalking.CurrentValue)
        {
            float targetAngle = CalculateTargetAngle(ref walkInput, ref cameraAngleY);
            float smoothedAngle = CalculateSmoothedAngle(playerAngleY, targetAngle);
            eulerAngles = new Vector3(0f, smoothedAngle, 0f);

            Vector3 targetForward = CalculateForwardVector(targetAngle);
            movement = MultiplyMovementVector(ref targetForward);
        }
        else
        {
            movement = Vector3.zero;
            eulerAngles = new Vector3(0f, playerAngleY, 0f);
        }

        return eulerAngles;
    }

    private float CalculateTargetAngle(ref Vector2 walkInput, ref float cameraAngle)
    {
        Vector3 inputDirection = new(walkInput.x, 0f, walkInput.y);
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        targetAngle += cameraAngle;

        return targetAngle;
    }

    private float CalculateSmoothedAngle(float playerAngle, float targetAngle) =>
        Mathf.LerpAngle(playerAngle, targetAngle, Time.deltaTime * _playerSettings.SlewSpeed);

    private Vector3 CalculateForwardVector(float targetAngle)
    {
        float radians = targetAngle * Mathf.Deg2Rad;
        Vector3 forward = new(Mathf.Sin(radians), 0f, Mathf.Cos(radians));

        return forward;
    }

    private Vector3 MultiplyMovementVector(ref Vector3 direction)
    {
        float speed = _playerState.IsRunning.CurrentValue
            ? _playerSettings.RunSpeed
            : _playerSettings.WalkSpeed;
        Vector3 movement = speed * Time.deltaTime * direction;

        return movement;
    }
}
