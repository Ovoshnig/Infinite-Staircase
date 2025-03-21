using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerHorizontalMover
{
    private readonly PlayerSettings _playerSettings;
    private readonly PlayerState _playerState;
    private readonly Transform _firstPersonCameraTransform;
    private readonly Transform _thirdPersonCameraTransform;
    private readonly CameraSwitch _cameraSwitch;

    public PlayerHorizontalMover(PlayerSettings playerSettings, 
        PlayerState playerState, CharacterController characterController, 
        CameraSwitch cameraSwitch)
    {
        _playerSettings = playerSettings;
        _playerState = playerState;
        _cameraSwitch = cameraSwitch;

        _firstPersonCameraTransform = characterController
            .GetComponentInChildren<CinemachineHardLockToTarget>()
            .transform;

        _thirdPersonCameraTransform = characterController
            .GetComponentInChildren<CinemachineOrbitalFollow>()
            .transform;
    }

    public Vector3 EulerAngles { get; private set; } = Vector3.zero;

    public virtual Vector3 CalculateMovementVector()
    {
        Vector3 movement;

        if (_playerState.IsWalking.CurrentValue)
        {
            float targetAngle = CalculateTargetAngle();
            float smoothedAngle = CalculateSmoothedAngle(targetAngle);
            EulerAngles = new Vector3(0f, smoothedAngle, 0f);

            Vector3 targetForward = CalculateForwardVector(targetAngle);
            movement = MultiplyMovementVector(targetForward);
        }
        else
        {
            movement = Vector3.zero;
        }

        return movement;
    }

    private float CalculateTargetAngle()
    {
        Vector3 inputDirection = new(_playerState.WalkInput.x, 0f, _playerState.WalkInput.y);
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

        targetAngle += _cameraSwitch.IsFirstPerson.CurrentValue
            ? _firstPersonCameraTransform.eulerAngles.y
            : _thirdPersonCameraTransform.eulerAngles.y;

        return targetAngle;
    }

    private float CalculateSmoothedAngle(float angel) =>
        Mathf.LerpAngle(_playerState.EulerAngels.y, angel, Time.deltaTime * _playerSettings.SlewSpeed);

    private Vector3 CalculateForwardVector(float angel)
    {
        float radians = angel * Mathf.Deg2Rad;
        Vector3 forward = new(Mathf.Sin(radians), 0f, Mathf.Cos(radians));

        return forward;
    }

    private Vector3 MultiplyMovementVector(Vector3 direction)
    {
        float speed = _playerState.IsRunning.CurrentValue 
            ? _playerSettings.RunSpeed 
            : _playerSettings.WalkSpeed;
        Vector3 movement = speed * Time.deltaTime * direction;

        return movement;
    }
}
