using System.ComponentModel;
using UnityEngine;
using Zenject;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] private float _xRotationLimitUp;
    [SerializeField] private float _xRotationLimitDown;
    [SerializeField] private PlayerState _playerState;

    private LookTuner _lookTuner;
    private float _rotationSpeed;
    private float _rotationX;

    [Inject]
    private void Construct(LookTuner lookTuner) => _lookTuner = lookTuner;

    private void Awake() => _lookTuner.PropertyChanged += OnPropertyChanged;

    private void Start() => _rotationSpeed = _lookTuner.Sensitivity;

    private void OnDestroy() => _lookTuner.PropertyChanged -= OnPropertyChanged;

    private void Update() => Look();

    private void Look()
    {
        _rotationX += -_playerState.LookInput.y * _rotationSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -_xRotationLimitDown, _xRotationLimitUp);
        transform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e) => 
        _rotationSpeed = _lookTuner.Sensitivity;
}
