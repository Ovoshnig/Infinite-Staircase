using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravity;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _xRotationLimit;
    [SerializeField] private bool _canMove = true;
    [SerializeField] private Transform _cameraTransform;

    private WindowTracker _windowTracker;
    private LookTuner _lookTuner;
    private CharacterController _characterController;
    private PlayerInput _playerInput;
    private Vector2 _movementInput;
    private Vector2 _lookInput;
    private bool _jumpInput;
    private bool _runInput;
    private Vector3 _moveDirection;
    private float _rotationX;
    private float _currentSpeedX;
    private float _currentSpeedZ;
    private float _movementDirectionY;

    [Inject]
    private void Construct(WindowTracker windowTracker, LookTuner lookTuner)
    {
        _windowTracker = windowTracker;
        _lookTuner = lookTuner;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _playerInput = new PlayerInput();
        _playerInput.Player.Move.performed += OnMovePerformed;
        _playerInput.Player.Look.performed += OnLookPerformed;
        _playerInput.Player.Jump.performed += OnJumpPerformed;
        _playerInput.Player.Run.performed += OnRunPerformed;
        _playerInput.Player.Move.canceled += OnMoveCanceled;
        _playerInput.Player.Look.canceled += OnLookCanceled;
        _playerInput.Player.Jump.canceled += OnJumpCanceled;
        _playerInput.Player.Run.canceled += OnRunCanceled;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _rotationSpeed = _lookTuner.Sensitivity;
    }

    private void OnEnable()
    {
        _playerInput.Enable();

        _windowTracker.WindowOpened += OnWindowOpened;
        _windowTracker.WindowClosed += OnWindowClosed;
    }

    private void OnDisable()
    {
        _playerInput.Disable();

        _windowTracker.WindowOpened -= OnWindowOpened;
        _windowTracker.WindowClosed -= OnWindowClosed;
    }

    #region "Performed actions"

    private void OnMovePerformed(InputAction.CallbackContext context) => _movementInput = context.action.ReadValue<Vector2>();

    private void OnLookPerformed(InputAction.CallbackContext context) => _lookInput = context.action.ReadValue<Vector2>();

    private void OnJumpPerformed(InputAction.CallbackContext context) => _jumpInput = true;
    
    private void OnRunPerformed(InputAction.CallbackContext context) => _runInput = true;
    
    #endregion

    #region "Canceled actions"

    private void OnMoveCanceled(InputAction.CallbackContext context) => _movementInput = Vector2.zero;
    
    private void OnLookCanceled(InputAction.CallbackContext context) => _lookInput = Vector2.zero;

    private void OnJumpCanceled(InputAction.CallbackContext context) => _jumpInput = false;
    
    private void OnRunCanceled(InputAction.CallbackContext context) => _runInput = false;

    #endregion

    private void OnWindowOpened() => _canMove = false;

    private void OnWindowClosed()
    {
        _canMove = true;
        _rotationSpeed = _lookTuner.Sensitivity;
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

        _currentSpeedX = (_runInput ? _runSpeed : _walkSpeed) * _movementInput.y;
        _currentSpeedZ = (_runInput ? _runSpeed : _walkSpeed) * _movementInput.x;
        _movementDirectionY = _moveDirection.y;
        _moveDirection = (forward * _currentSpeedX) + (right * _currentSpeedZ);
    }

    private void Jump()
    {
        if (_jumpInput && _characterController.isGrounded)
            _moveDirection.y = _jumpForce;
        else
            _moveDirection.y = _movementDirectionY;

        if (!_characterController.isGrounded)
            _moveDirection.y -= _gravity * Time.deltaTime;

        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    private void Look()
    {
        _rotationX += -_lookInput.y * _rotationSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -_xRotationLimit, _xRotationLimit);
        _cameraTransform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
        transform.rotation *= Quaternion.Euler(0f, _lookInput.x * _rotationSpeed, 0f);
    }
}
