using R3;
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
    private CompositeDisposable _compositeDisposable;

    [Inject]
    private void Construct(LookTuner lookTuner) => _lookTuner = lookTuner;

    private void Awake()
    {
        var sensitivityDisposable = _lookTuner.Sensitivity
            .Subscribe(value => _rotationSpeed = value);

        var lookDisposable = _playerState.IsLooking
            .Where(value => value)
            .Subscribe(_ =>
            {
                _rotationX += -_playerState.LookInput.y * _rotationSpeed;
                _rotationX = Mathf.Clamp(_rotationX, -_xRotationLimitDown, _xRotationLimitUp);
                transform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
            });

        _compositeDisposable = new CompositeDisposable()
        {
            sensitivityDisposable,
            lookDisposable
        };
    }

    private void OnDestroy() => _compositeDisposable?.Dispose();
}
