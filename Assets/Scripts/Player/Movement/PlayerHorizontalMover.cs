using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerHorizontalMover : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _walkSpeed;
    [SerializeField, Min(0f)] private float _runSpeed;
    [SerializeField, Min(1f)] private float _slewSpeed = 1f;

    private PlayerState _playerState;
    private FirstPersonLook _firstPersonLook;
    private ThirdPersonLook _thirdPersonLook;
    private CharacterController _characterController;

    [Inject]
    protected void Construct([Inject(Id = BindConstants.PlayerId)] PlayerState playerState,
        [Inject(Id = BindConstants.FirstPersonCameraId)] FirstPersonLook firstPersonLook,
        [Inject(Id = BindConstants.ThirdPersonCameraId)] ThirdPersonLook thirdPersonLook)
    {
        _playerState = playerState;
        _firstPersonLook = firstPersonLook;
        _thirdPersonLook = thirdPersonLook;
    }

    protected virtual void Awake() => _characterController = GetComponent<CharacterController>();

    protected virtual void Update() => Move();

    protected virtual void Move()
    {
        if (_playerState.IsWalking.CurrentValue)
        {
            float targetAngle = CalculateTargetAngel();
            float smoothedAngle = CalculateSmothedVector(targetAngle);
            transform.eulerAngles = new Vector3(0f, smoothedAngle, 0f);

            Vector3 targetForward = CalculateForwardVector(targetAngle);
            Vector3 motion = CalculateMotionVector(targetForward);
            _characterController.Move(motion * Time.deltaTime);
        }
    }

    private float CalculateTargetAngel()
    {
        Vector3 inputDirection = new(_playerState.WalkInput.x, 0f, _playerState.WalkInput.y);
        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

        targetAngle += _firstPersonLook.IsSelected
            ? _firstPersonLook.transform.eulerAngles.y
            : _thirdPersonLook.transform.eulerAngles.y;

        return targetAngle;
    }

    private float CalculateSmothedVector(float angel) =>
        Mathf.LerpAngle(transform.eulerAngles.y, angel, Time.deltaTime * _slewSpeed);

    private Vector3 CalculateForwardVector(float angel)
    {
        float radians = angel * Mathf.Deg2Rad;
        Vector3 forward = new(Mathf.Sin(radians), 0f, Mathf.Cos(radians));

        return forward;
    }

    private Vector3 CalculateMotionVector(Vector3 direction)
    {
        float speed = _playerState.IsRunning.CurrentValue ? _runSpeed : _walkSpeed;
        Vector3 motion = direction * speed;

        return motion;
    }
}
