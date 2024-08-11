using System.ComponentModel;
using UnityEngine;
using Zenject;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] private float _xRotationLimitUp;
    [SerializeField] private float _xRotationLimitDown;
    [SerializeField] private PlayerState _playerState;

    private LookTuner _lookTuner;
    private PauseMenu _pauseMenu;
    private float _rotationSpeed;
    private float _rotationX;

    [Inject]
    private void Construct(LookTuner lookTuner, PauseMenu pauseMenu)
    {
        _lookTuner = lookTuner;
        _pauseMenu = pauseMenu;
    }

    private void Awake() => _pauseMenu.Resumed += OnResumed;

    private void Start() => _rotationSpeed = _lookTuner.Sensitivity;

    private void OnDestroy() => _pauseMenu.Resumed -= OnResumed;

    private void Update() => Look();

    private void Look()
    {
        _rotationX += -_playerState.LookInput.y * _rotationSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -_xRotationLimitDown, _xRotationLimitUp);
        transform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
    }

    private void OnResumed() => _rotationSpeed = _lookTuner.Sensitivity;
}
