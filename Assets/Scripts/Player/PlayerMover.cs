using System.ComponentModel;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController),
                  typeof(PlayerState))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravity;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _xRotationLimit;
    [SerializeField] private bool _canMove = true;
    [SerializeField] private Transform _cameraTransform;

    private CharacterController _characterController;
    private PlayerState _playerState;
    private LookTuner _lookTuner;
    private Vector3 _moveDirection;
    private float _rotationX;
    private float _currentSpeedX;
    private float _currentSpeedZ;
    private float _movementDirectionY;

    [Inject]
    private void Construct(LookTuner lookTuner) => _lookTuner = lookTuner;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerState = GetComponent<PlayerState>();

        _playerState.JumpStarted += OnJumpStarted;
        _lookTuner.PropertyChanged += OnPropertyChanged;
    }

    private void Start() => _rotationSpeed = _lookTuner.Sensitivity;

    private void OnDisable()
    {
        _playerState.JumpStarted -= OnJumpStarted;
        _lookTuner.PropertyChanged -= OnPropertyChanged;
    }

    private void Update()
    {
        if (_canMove)
        {
            Move();
            Jump();
            Look();
        }
    }

    private void Move()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        _currentSpeedX = _playerState.WalkInput.y;
        _currentSpeedX *= _playerState.IsRunning ? _runSpeed : _walkSpeed;
        _currentSpeedZ = _playerState.WalkInput.x;
        _currentSpeedZ *= _playerState.IsRunning ? _runSpeed : _walkSpeed;

        _movementDirectionY = _moveDirection.y;
        _moveDirection = (forward * _currentSpeedX) + (right * _currentSpeedZ);
    }

    private void Jump()
    {
        _moveDirection.y = _movementDirectionY;

        if (_playerState.IsInAir)
            _moveDirection.y -= _gravity * Time.deltaTime;

        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    private void Look()
    {
        _rotationX += -_playerState.LookInput.y * _rotationSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -_xRotationLimit, _xRotationLimit);
        _cameraTransform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
        transform.rotation *= Quaternion.Euler(0f, _playerState.LookInput.x * _rotationSpeed, 0f);
    }

    private void OnJumpStarted() => _moveDirection.y = _jumpForce;

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e) => 
        _rotationSpeed = _lookTuner.Sensitivity;
}
