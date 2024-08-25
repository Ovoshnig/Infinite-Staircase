using R3;
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

    private CharacterController _characterController;
    private PlayerState _playerState;
    private LookTuner _lookTuner;
    private Vector3 _moveDirection;
    private float _rotationSpeed;
    private float _movementDirectionY;
    private CompositeDisposable _compositiveDisposable;

    [Inject]
    private void Construct(LookTuner lookTuner) => _lookTuner = lookTuner;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerState = GetComponent<PlayerState>();

        var lookDisposable = _playerState.IsLooking
            .Where(value => value)
            .Subscribe(_ =>
                transform.rotation *= Quaternion.Euler(0f, _playerState.LookInput.x * _rotationSpeed, 0f)
            );

        var jumpDisposable = _playerState.IsJumping
            .Where(value => value)
            .Subscribe(_ => _moveDirection.y = _jumpForce);

        var groundEnterDisposable = _playerState.IsGrounded
            .Where(value => value)
            .Subscribe(_ => _moveDirection.y = -_gravity);

        var sensitivityDisposable = _lookTuner.SensitivityReactive
            .Subscribe(value => _rotationSpeed = value);

        _compositiveDisposable = new CompositeDisposable
        {
            jumpDisposable,
            groundEnterDisposable,
            sensitivityDisposable,
        };
    }

    private void OnDestroy() => _compositiveDisposable?.Dispose();

    private void Update()
    {
        Move();
        Fall();
    }

    private void Move()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        var currentSpeedX = _playerState.WalkInput.y;
        currentSpeedX *= _playerState.IsRunning.CurrentValue ? _runSpeed : _walkSpeed;
        var currentSpeedZ = _playerState.WalkInput.x;
        currentSpeedZ *= _playerState.IsRunning.CurrentValue ? _runSpeed : _walkSpeed;

        _movementDirectionY = _moveDirection.y;
        _moveDirection = (forward * currentSpeedX) + (right * currentSpeedZ);
    }

    private void Fall()
    {
        _moveDirection.y = _movementDirectionY;

        if (!_playerState.IsGrounded.CurrentValue)
            _moveDirection.y -= _gravity * Time.deltaTime;

        _characterController.Move(_moveDirection * Time.deltaTime);
    }
}
