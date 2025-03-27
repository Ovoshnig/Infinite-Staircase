using R3;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoverView : MonoBehaviour
{
    private PlayerHorizontalCalculator _playerHorizontalCalculator;
    private PlayerVerticalCalculator _playerVerticalCalculator;
    private CharacterController _characterController;
    private PlayerState _playerState;
    private Transform _firstPersonCameraTransform;
    private Transform _thirdPersonCameraTransform;
    private CameraSwitch _cameraSwitch;
    private PlayerSettings _playerSettings;
    private Vector3 _horizontalMovement = Vector3.zero;
    private Vector3 _verticalMovement;

    [Inject]
    public void Construct(PlayerState playerState, FirstCameraPriorityChanger firstCamera,
        ThirdCameraPriorityChanger thirdCamera, CameraSwitch cameraSwitch, PlayerSettings playerSettings)
    {
        _playerState = playerState;
        _firstPersonCameraTransform = firstCamera.transform;
        _thirdPersonCameraTransform = thirdCamera.transform;
        _cameraSwitch = cameraSwitch;
        _playerSettings = playerSettings;

        _playerHorizontalCalculator = new PlayerHorizontalCalculator(_playerSettings, _playerState);
        _playerVerticalCalculator = new PlayerVerticalCalculator(_playerSettings, _playerState);
    }

    private void Awake() => _characterController = GetComponent<CharacterController>();

    private void Start()
    {
        _playerState.IsJumping
            .Where(value => value)
            .Subscribe(_ => _playerVerticalCalculator.CalculateJumping(ref _verticalMovement))
            .AddTo(this);
        _playerState.IsGrounded
            .Where(value => value)
            .Subscribe(_ => _playerVerticalCalculator.CalculateLanding(ref _verticalMovement))
            .AddTo(this);
    }

    private void Update()
            {
                float playerAngleY = _playerState.EulerAngels.y;
                float cameraAngleY = _cameraSwitch.IsFirstPerson.CurrentValue
                    ? _firstPersonCameraTransform.eulerAngles.y
                    : _thirdPersonCameraTransform.eulerAngles.y;

                Vector3 eulerAngles = _playerHorizontalCalculator
                    .CalculateHorizontalVector(ref _horizontalMovement, _playerState.WalkInput,
                    playerAngleY, cameraAngleY);
                Vector3 fallingVector = _playerVerticalCalculator.CalculateFallingVector(ref _verticalMovement);

                transform.eulerAngles = eulerAngles;
                _characterController.Move(_horizontalMovement);
                _characterController.Move(fallingVector);
    }
}
