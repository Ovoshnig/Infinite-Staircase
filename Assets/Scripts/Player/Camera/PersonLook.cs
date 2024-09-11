using R3;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CinemachineCamera))]
public abstract class PersonLook : MonoBehaviour
{
    private CinemachineCamera _cinemachineCamera;
    private CameraSwitch _cameraSwitch;
    private Transform _playerTransform;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private LookTuner _lookTuner;

    [Inject]
    protected void Construct([Inject(Id = ZenjectIdConstants.PlayerId)] CameraSwitch cameraSwitch,
        [Inject(Id = ZenjectIdConstants.PlayerId)] PlayerState playerState,
        [Inject(Id = ZenjectIdConstants.PlayerId)] SkinnedMeshRenderer skinnedMeshRenderer,
        LookTuner lookTuner)
    {
        _cameraSwitch = cameraSwitch;
        _playerTransform = playerState.transform;
        _skinnedMeshRenderer = skinnedMeshRenderer;
        _lookTuner = lookTuner;
    }

    public CinemachineCamera CinemachineCamera => _cinemachineCamera;
    public bool IsSelected { get; protected set; } = false;

    protected virtual Transform FollowPoint { get; } = null;
    protected Transform PlayerTransform => _playerTransform;
    protected SkinnedMeshRenderer SkinnedMeshRenderer => _skinnedMeshRenderer;
    protected CameraSwitch CameraSwitch => _cameraSwitch;
    protected LookTuner LookTuner => _lookTuner;
    protected CompositeDisposable PermanentCompositeDisposable { get; private set; }
    protected CompositeDisposable EnablingCompositeDisposable { get; private set; }

    protected virtual void Awake()
    {
        _cinemachineCamera = GetComponent<CinemachineCamera>();

        PermanentCompositeDisposable = new CompositeDisposable();
    }

    protected virtual void OnEnable() => EnablingCompositeDisposable = new CompositeDisposable();

    protected virtual void Start() => _cinemachineCamera.Follow = FollowPoint;

    protected virtual void OnDisable() => EnablingCompositeDisposable?.Dispose();

    protected virtual void OnDestroy() => PermanentCompositeDisposable?.Dispose();
}
