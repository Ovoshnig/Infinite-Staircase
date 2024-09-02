using R3;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public abstract class PersonLook : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField, Range(0f, 180f)] private float _xRotationLimitUp;
    [SerializeField, Range(0f, 180f)] private float _xRotationLimitDown;
    [SerializeField, Min(1f)] private float _slewSpeed = 1f;

    private Transform _playerTransform;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private CameraSwitch _cameraSwitch;
    private PlayerState _playerState;
    private LookTuner _lookTuner;
    private float _rotationSpeed = 1.0f;
    private float _rotationX = 0f;
    private float _rotationY = 0f;

    [Inject]
    protected void Construct([Inject(Id = BindConstants.PlayerId)] CameraSwitch cameraSwitch,
        [Inject(Id = BindConstants.PlayerId)] CharacterController characterController,
        [Inject(Id = BindConstants.PlayerId)] SkinnedMeshRenderer skinnedMeshRenderer,
        PlayerState playerState, LookTuner lookTuner)
    {
        _cameraSwitch = cameraSwitch;
        _playerTransform = characterController.transform;
        _skinnedMeshRenderer = skinnedMeshRenderer;
        _playerState = playerState;
        _lookTuner = lookTuner;
    }

    protected virtual Transform FollowPoint { get; } = null;
    protected Transform PlayerTransform => _playerTransform;
    protected SkinnedMeshRenderer SkinnedMeshRenderer => _skinnedMeshRenderer;
    protected CameraSwitch CameraSwitch => _cameraSwitch;
    protected CompositeDisposable CompositeDisposable { get; private set; }

    protected virtual void Awake() => _camera.Follow = FollowPoint;

    protected virtual void OnEnable()
    {
        var sensitivityDisposable = _lookTuner.Sensitivity
            .Subscribe(value => _rotationSpeed = value);

        var lookDisposable = _playerState.IsLooking
            .Where(value => value)
            .Subscribe(_ =>
            {
                _rotationX -= _playerState.LookInput.y * _rotationSpeed;
                _rotationX = Mathf.Clamp(_rotationX, -_xRotationLimitDown, _xRotationLimitUp);
                _rotationY += _playerState.LookInput.x * _rotationSpeed;
                transform.localRotation = Quaternion.Euler(_rotationX, _rotationY, 0f);
            });

        CompositeDisposable = new CompositeDisposable()
        {
            sensitivityDisposable,
            lookDisposable
        };
    }

    protected virtual void OnDisable() => CompositeDisposable?.Dispose();

    protected virtual void OnDestroy()
    {
    }

    protected virtual void Update()
    {
        transform.position = FollowPoint.position;

        if (_playerState.IsWalking.CurrentValue)
        {
            Vector3 inputDirection = new(_playerState.WalkInput.x, 0, _playerState.WalkInput.y);
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            targetAngle += transform.eulerAngles.y;
            float smoothedAngle = Mathf.LerpAngle(PlayerTransform.eulerAngles.y, targetAngle, Time.deltaTime * _slewSpeed);
            PlayerTransform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
        }
    }
}
