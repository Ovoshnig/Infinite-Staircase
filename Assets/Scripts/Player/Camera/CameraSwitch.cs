using R3;
using UnityEngine;
using Unity.Cinemachine;
using Zenject;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _playerScope;

    private readonly ReactiveProperty<bool> _isFirstPerson = new(true);
    private CinemachineCamera _firstPersonCamera;
    private CinemachineCamera _thirdPersonCamera;
    private PlayerInputHandler _inputHandler;
    private WindowTracker _windowTracker;
    private CompositeDisposable _compositeDisposable;

    [Inject]
    private void Construct(PlayerInputHandler inputHandler, WindowTracker windowTracker,
        [Inject(Id = ZenjectIdConstants.FirstPersonCameraId)] FirstPersonLook firstPersonLook,
        [Inject(Id = ZenjectIdConstants.ThirdPersonCameraId)] ThirdPersonLook thirdPersonLook)
    {
        _inputHandler = inputHandler;
        _windowTracker = windowTracker;
        _firstPersonCamera = firstPersonLook.CinemachineCamera;
        _thirdPersonCamera = thirdPersonLook.CinemachineCamera;
    }

    public ReadOnlyReactiveProperty<bool> IsFirstPerson => _isFirstPerson;

    private void Awake()
    {
        var togglePerspectiveDisposable = _inputHandler.IsTogglePerspectivePressed
            .Where(value => value)
            .Subscribe(_ => _isFirstPerson.Value = !_isFirstPerson.Value);

        var cameraChangeDisposable = _isFirstPerson
            .Subscribe(value => SetCamera(value));

        var windowDisposable = _windowTracker.IsOpen
            .Where(_ => _isFirstPerson.Value)
            .Subscribe(value => _playerScope.SetActive(!value));

        _compositeDisposable = new CompositeDisposable()
        {
            togglePerspectiveDisposable,
            cameraChangeDisposable,
            windowDisposable
        };
    }

    private void OnDestroy() => _compositeDisposable?.Dispose();

    private void SetCamera(bool isFirstPerson)
    {
        _firstPersonCamera.Priority = isFirstPerson ? 1 : 0;
        _thirdPersonCamera.Priority = isFirstPerson ? 0 : 1;

        _playerScope.SetActive(isFirstPerson);
    }
}
