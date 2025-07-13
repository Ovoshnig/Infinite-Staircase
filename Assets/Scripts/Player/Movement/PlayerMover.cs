using R3;
using System;
using UnityEngine;
using VContainer.Unity;

public class PlayerMover : IInitializable, ITickable, IDisposable
{
    private readonly PlayerState _playerState;
    private readonly Transform _firstCameraTransform;
    private readonly Transform _thirdCameraTransform;
    private readonly CameraSwitch _cameraSwitch;
    private readonly PlayerSettings _playerSettings;
    private readonly PlayerHorizontalCalculator _playerHorizontalCalculator;
    private readonly PlayerVerticalCalculator _playerVerticalCalculator;
    private readonly ReactiveProperty<Vector3> _horizontalMotion = new(Vector3.zero);
    private readonly ReactiveProperty<Vector3> _fallingMotion = new(Vector3.zero);
    private readonly ReactiveProperty<Vector3> _eulerAngles = new(Vector3.zero);
    private readonly CompositeDisposable _compositeDisposable = new();

    private Vector3 _horizontalMovement = Vector3.zero;
    private Vector3 _verticalMovement = Vector3.zero;

    public PlayerMover(PlayerState playerState, FirstCameraPriorityView firstCamera,
        ThirdCameraPriorityView thirdCamera, CameraSwitch cameraSwitch, PlayerSettings playerSettings)
    {
        _playerState = playerState;
        _firstCameraTransform = firstCamera.transform;
        _thirdCameraTransform = thirdCamera.transform;
        _cameraSwitch = cameraSwitch;
        _playerSettings = playerSettings;

        _playerHorizontalCalculator = new PlayerHorizontalCalculator(_playerSettings, _playerState);
        _playerVerticalCalculator = new PlayerVerticalCalculator(_playerSettings, _playerState);
    }

    public ReadOnlyReactiveProperty<Vector3> HorizontalMotion => _horizontalMotion;
    public ReadOnlyReactiveProperty<Vector3> FallingMotion => _fallingMotion;
    public ReadOnlyReactiveProperty<Vector3> EulerAngles => _eulerAngles;

    public void Initialize()
    {
        _playerState.IsGrounded
            .Where(isGrounded => isGrounded)
            .Subscribe(_ => _playerVerticalCalculator.CalculateLanding(ref _verticalMovement))
            .AddTo(_compositeDisposable);
        _playerState.Jumped
            .Subscribe(_ => _playerVerticalCalculator.CalculateJumping(ref _verticalMovement))
            .AddTo(_compositeDisposable);
    }

    public void Tick()
    {
        float playerAngleY = _playerState.EulerAngles.y;
        float cameraAngleY = _cameraSwitch.IsFirstPerson.CurrentValue
            ? _firstCameraTransform.eulerAngles.y
            : _thirdCameraTransform.eulerAngles.y;

        Vector3 eulerAngles = _playerHorizontalCalculator
            .CalculateHorizontalVector(ref _horizontalMovement, _playerState.WalkInput,
            playerAngleY, cameraAngleY);
        Vector3 fallingVector = _playerVerticalCalculator.CalculateFalling(ref _verticalMovement);

        _horizontalMotion.Value = _horizontalMovement;
        _fallingMotion.Value = fallingVector;
        _eulerAngles.Value = eulerAngles;
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
