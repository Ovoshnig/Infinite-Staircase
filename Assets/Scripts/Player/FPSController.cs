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

    private GamePauser _gamePauser;
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
    private void Construct(GamePauser gamePauser, LookTuner lookTuner)
    {
        _gamePauser = gamePauser;
        _lookTuner = lookTuner;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _playerInput = new PlayerInput();

        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Look.performed += OnLook;
        _playerInput.Player.Jump.performed += OnJump;
        _playerInput.Player.Run.performed += OnRun;

        _playerInput.Player.Move.canceled += OnMoveCancel;
        _playerInput.Player.Look.canceled += OnLookCancel;
        _playerInput.Player.Jump.canceled += OnJumpCancel;
        _playerInput.Player.Run.canceled += OnRunCancel;
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

        _gamePauser.GamePaused += Pause;
        _gamePauser.GameUnpaused += Unpause;
    }

    private void OnDisable()
    {
        _playerInput.Disable();

        _gamePauser.GamePaused -= Pause;
        _gamePauser.GameUnpaused -= Unpause;
    }

    #region "Performed actions"

    private void OnMove(InputAction.CallbackContext context) => _movementInput = context.action.ReadValue<Vector2>();

    private void OnLook(InputAction.CallbackContext context) => _lookInput = context.action.ReadValue<Vector2>();

    private void OnJump(InputAction.CallbackContext context) => _jumpInput = true;
    
    private void OnRun(InputAction.CallbackContext context) => _runInput = true;
    
    #endregion

    #region "Canceled actions"

    private void OnMoveCancel(InputAction.CallbackContext context) => _movementInput = Vector2.zero;
    
    private void OnLookCancel(InputAction.CallbackContext context) => _lookInput = Vector2.zero;

    private void OnJumpCancel(InputAction.CallbackContext context) => _jumpInput = false;
    
    private void OnRunCancel(InputAction.CallbackContext context) => _runInput = false;

    #endregion

    private void Pause()
    {
        _canMove = false;
        SetPauseState(pause: true);
    }

    private void Unpause()
    {
        _canMove = true;
        SetPauseState(pause: false);
        _rotationSpeed = _lookTuner.Sensitivity;
    }

    private void SetPauseState(bool pause)
    {
        Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = pause;
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
        _cameraTransform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, _lookInput.x * _rotationSpeed, 0);
    }
}
