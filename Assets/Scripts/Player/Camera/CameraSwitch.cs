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
    private bool _isFirstPerson = true;

    [Inject]
    private void Construct(PlayerInputHandler inputHandler, WindowTracker windowTracker)
    {
        _inputHandler = inputHandler;
        _windowTracker = windowTracker;
    }

    private void Awake()
    {
        _inputHandler.TogglePerspectivePerformed += OnTogglePerspectivePerformed;

        _windowTracker.WindowOpened += OnWindowOpened;
        _windowTracker.WindowClosed += OnWindowClosed;
    }

    private void Start() => SetCamera(_isFirstPerson);

    private void OnDestroy()
    {
        _inputHandler.TogglePerspectivePerformed -= OnTogglePerspectivePerformed;

        _windowTracker.WindowOpened -= OnWindowOpened;
        _windowTracker.WindowClosed -= OnWindowClosed;
    }

    private void OnTogglePerspectivePerformed()
    {
        _isFirstPerson = !_isFirstPerson;
        SetCamera(_isFirstPerson);
    }

    private void OnWindowOpened()
    {
        if (_isFirstPerson)
            _playerScope.SetActive(false);
    }

    private void OnWindowClosed()
    {
        if (_isFirstPerson)
            _playerScope.SetActive(true);
    }

    private void SetCamera(bool isFirstPerson)
    {
        _firstPersonCamera.Priority = isFirstPerson ? 1 : 0;
        _thirdPersonCamera.Priority = isFirstPerson ? 0 : 1;
        _playerScope.SetActive(isFirstPerson);
    }
}
