using R3;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(CinemachineCamera))]
public abstract class PersonLook : MonoBehaviour
{
    private CinemachineCamera _cinemachineCamera;
    private Transform _playerTransform;

    [Inject]
    public void Construct(CharacterController characterController) => 
        _playerTransform = characterController.transform;

    public CinemachineCamera CinemachineCamera => _cinemachineCamera;

    protected virtual Transform FollowPoint { get; } = null;
    protected Transform PlayerTransform => _playerTransform;
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
