using R3;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public abstract class PersonLook : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private float _xRotationLimitUp;
    [SerializeField] private float _xRotationLimitDown;

    private Transform _playerTransform;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private CameraSwitch _cameraSwitch;
    private PlayerState _playerState;
    private LookTuner _lookTuner;

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

    protected CinemachineCamera Camera => _camera;
    protected float XRotationLimitUp => _xRotationLimitUp;
    protected float XRotationLimitDown => _xRotationLimitDown;
    protected Transform PlayerTransform => _playerTransform;
    protected SkinnedMeshRenderer SkinnedMeshRenderer => _skinnedMeshRenderer;
    protected CameraSwitch CameraSwitch => _cameraSwitch;
    protected PlayerState PlayerState => _playerState;
    protected LookTuner LookTuner => _lookTuner;
    protected float RotationX { get; set; } = 0f;
    protected float RotationY { get; set; } = 0f;
    protected float RotationSpeed { get; private set; } = 1.0f;
    protected CompositeDisposable CompositeDisposable { get; private set; }

    protected virtual void Awake() 
    { 
    }

    protected virtual void OnEnable()
    {
        var sensitivityDisposable = LookTuner.Sensitivity
            .Subscribe(value => RotationSpeed = value);

        CompositeDisposable = new CompositeDisposable()
        {
            sensitivityDisposable
        };
    }

    protected virtual void OnDisable() => CompositeDisposable?.Dispose();

    protected virtual void OnDestroy()
    {
    }

    protected virtual void Update()
    {
    }
}
