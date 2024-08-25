using R3;
using UnityEngine;
using Unity.Cinemachine;
using Zenject;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _firstPersonCamera;
    [SerializeField] private CinemachineCamera _thirdPersonCamera;
    [SerializeField] private GameObject _playerScope;

    private PlayerInputHandler _inputHandler;
    private WindowTracker _windowTracker;
    private CompositeDisposable _compositeDisposable;
    private bool _isFirstPerson = true;

    [Inject]
    private void Construct(PlayerInputHandler inputHandler, WindowTracker windowTracker)
    {
        _inputHandler = inputHandler;
        _windowTracker = windowTracker;
    }

    private void Awake()
    {
        var togglePerspectiveDisposable = _inputHandler.IsTogglePerspectivePressed
            .Where(value => value)
            .Subscribe(_ =>
            {
                _isFirstPerson = !_isFirstPerson;
                SetCamera(_isFirstPerson);
            });

        var windowDisposable = _windowTracker.IsOpen
            .Subscribe(value => 
            {
                if (_isFirstPerson)
                    _playerScope.SetActive(!value);
            });

        _compositeDisposable = new CompositeDisposable()
        {
            togglePerspectiveDisposable,
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
