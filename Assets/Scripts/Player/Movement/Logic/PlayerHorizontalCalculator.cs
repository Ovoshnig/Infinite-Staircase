using UnityEngine;

public readonly struct HorizontalMovementResult
{
    public Vector3 Velocity { get; }
    public float AngleY { get; }

    public HorizontalMovementResult(Vector3 velocity, float angleY)
    {
        Velocity = velocity;
        AngleY = angleY;
    }
}

public class PlayerHorizontalMovementCalculator
{
    private readonly PlayerSettings _playerSettings;

    public PlayerHorizontalMovementCalculator(PlayerSettings playerSettings) => 
        _playerSettings = playerSettings;

    public HorizontalMovementResult Calculate(Vector2 walkInput, float playerAngleY, float cameraAngleY, 
        bool isWalking, bool isRunning)
    {
        if (!isWalking)
            return new HorizontalMovementResult(Vector3.zero, playerAngleY);

        float targetAngle = CalculateTargetAngle(walkInput, cameraAngleY);
        float smoothedAngle = CalculateSmoothedAngle(playerAngleY, targetAngle);

        Vector3 direction = CalculateForwardVector(targetAngle);

        float speed = isRunning
            ? _playerSettings.RunSpeed
            : _playerSettings.WalkSpeed;
        Vector3 velocity = direction * speed;

        return new HorizontalMovementResult(velocity, smoothedAngle);
    }

    private float CalculateTargetAngle(Vector2 walkInput, float cameraAngle)
    {
        Vector3 inputDirection = new Vector3(walkInput.x, 0f, walkInput.y).normalized;
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        targetAngle += cameraAngle;
        return targetAngle;
    }

    private float CalculateSmoothedAngle(float playerAngle, float targetAngle)
    {
        float maxDegreesDelta = _playerSettings.SlewSpeed * Time.deltaTime;
        return Mathf.MoveTowardsAngle(playerAngle, targetAngle, maxDegreesDelta);
    }

    private Vector3 CalculateForwardVector(float targetAngle)
    {
        float radians = targetAngle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radians), 0f, Mathf.Cos(radians));
    }
}
