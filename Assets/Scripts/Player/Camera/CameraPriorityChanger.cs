using R3;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(CinemachineCamera))]
public abstract class CameraPriorityChanger : MonoBehaviour
{
    private CameraSwitch _cameraSwitch;
    private CinemachineCamera _camera;
    private readonly CompositeDisposable _compositeDisposable = new();

    [Inject]
    public void Construct(CameraSwitch cameraSwitch) => _cameraSwitch = cameraSwitch;

    public CameraSwitch CameraSwitch => _cameraSwitch;
    public CinemachineCamera Camera => _camera;
    public CompositeDisposable CompositeDisposable => _compositeDisposable;

    protected virtual void Awake() => _camera = GetComponent<CinemachineCamera>();

    protected virtual void Start() { }

    protected virtual void OnDestroy() => _compositeDisposable?.Dispose();
}
