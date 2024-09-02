using R3;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public abstract class PersonLook : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField, Range(0f, 180f)] private float _xRotationLimitUp;
    [SerializeField, Range(0f, 180f)] private float _xRotationLimitDown;

    private CameraSwitch _cameraSwitch;
    private Transform _playerTransform;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private PlayerState _playerState;
    private LookTuner _lookTuner;
    private float _rotationSpeed = 1.0f;
    private float _rotationX = 0f;
    private float _rotationY = 0f;

    [Inject]
    protected void Construct([Inject(Id = BindConstants.PlayerId)] CameraSwitch cameraSwitch,
        [Inject(Id = BindConstants.PlayerId)] PlayerState playerState,
        [Inject(Id = BindConstants.PlayerId)] SkinnedMeshRenderer skinnedMeshRenderer,
        LookTuner lookTuner)
    {
        _cameraSwitch = cameraSwitch;
        _playerState = playerState;
        _playerTransform = playerState.transform;
        _skinnedMeshRenderer = skinnedMeshRenderer;
        _lookTuner = lookTuner;
    }

    public CinemachineCamera Camera => _camera;
    public bool IsSelected { get; protected set; } = false;

    protected virtual Transform FollowPoint { get; } = null;
    protected Transform PlayerTransform => _playerTransform;
    protected SkinnedMeshRenderer SkinnedMeshRenderer => _skinnedMeshRenderer;
    protected CameraSwitch CameraSwitch => _cameraSwitch;
    protected CompositeDisposable PermanentCompositeDisposable { get; private set; }
    protected CompositeDisposable EnablingCompositeDisposable { get; private set; }

    protected virtual void Awake()
    {
        var lookDisposable = _playerState.IsLooking
            .Where(value => value)
            .Subscribe(_ =>
            {
                _rotationX -= _playerState.LookInput.y * _rotationSpeed;
                _rotationX = Mathf.Clamp(_rotationX, -_xRotationLimitDown, _xRotationLimitUp);
                _rotationY += _playerState.LookInput.x * _rotationSpeed;
                transform.localRotation = Quaternion.Euler(_rotationX, _rotationY, 0f);
            });

        PermanentCompositeDisposable = new CompositeDisposable()
        {
            lookDisposable
        };
    }

    protected virtual void OnEnable()
    {
        var sensitivityDisposable = _lookTuner.Sensitivity
            .Subscribe(value => _rotationSpeed = value);

        EnablingCompositeDisposable = new CompositeDisposable()
        {
            sensitivityDisposable
        };
    }

    protected virtual void Start() => _camera.Follow = FollowPoint;

    protected virtual void OnDisable() => EnablingCompositeDisposable?.Dispose();

    protected virtual void OnDestroy() => PermanentCompositeDisposable?.Dispose();

    protected virtual void Update() => transform.position = FollowPoint.position;
}
