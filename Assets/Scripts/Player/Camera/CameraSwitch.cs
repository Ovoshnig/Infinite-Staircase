using UnityEngine;
using Unity.Cinemachine;
using Zenject;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _firstPersonCamera;
    [SerializeField] private CinemachineCamera _thirdPersonCamera;

    private PlayerInputHandler _inputHandler;
    private GameObject _playerScope;
    private bool _isFirstPerson = true;

    [Inject]
    private void Construct(PlayerInputHandler inputHandler, 
        [Inject(Id = BindingConstants.PlayerScopeId)] GameObject playerScope)
    {
        _inputHandler = inputHandler;
        _playerScope = playerScope;
    }

    private void Awake() => _inputHandler.TogglePerspectivePerformed += OnTogglePerspectivePerformed;

    private void Start() => SetCamera(_isFirstPerson);

    private void OnDestroy() => _inputHandler.TogglePerspectivePerformed -= OnTogglePerspectivePerformed;

    private void OnTogglePerspectivePerformed()
    {
        _isFirstPerson = !_isFirstPerson;
        SetCamera(_isFirstPerson);
    }

    private void SetCamera(bool isFirstPerson)
    {
        _firstPersonCamera.Priority = isFirstPerson ? 1 : 0;
        _thirdPersonCamera.Priority = isFirstPerson ? 0 : 1;
        _playerScope.SetActive(isFirstPerson);
    }
}