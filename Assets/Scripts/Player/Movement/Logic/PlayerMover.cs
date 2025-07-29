using R3;
using System;
using UnityEngine;
using VContainer.Unity;

public class PlayerMover : IInitializable, ITickable, IDisposable
{
    private readonly PlayerState _playerState;
    private readonly Transform _cameraTransform;
    private readonly PlayerSettings _playerSettings;
    private readonly PlayerHorizontalMovementCalculator _horizontalCalculator;
    private readonly ReactiveProperty<Vector3> _frameMotion = new(Vector3.zero);
    private readonly ReactiveProperty<Vector3> _eulerAngles = new(Vector3.zero);
    private readonly CompositeDisposable _compositeDisposable = new();

    private Vector3 _velocity;
    private bool _isPause = false;

    public PlayerMover(PlayerState playerState, CameraPriorityView cameraPriorityView,
        PlayerSettings playerSettings)
    {
        _playerState = playerState;
        _cameraTransform = cameraPriorityView.transform;
        _playerSettings = playerSettings;

        _horizontalCalculator = new PlayerHorizontalMovementCalculator(_playerSettings);
    }

    public ReadOnlyReactiveProperty<Vector3> FrameMotion => _frameMotion;
    public ReadOnlyReactiveProperty<Vector3> EulerAngles => _eulerAngles;

    public void Initialize()
    {
        _playerState.Jumped
            .Subscribe(_ =>
            {
                if (_isPause)
                    return;

                _velocity.y = _playerSettings.JumpForce;
            })
            .AddTo(_compositeDisposable);
    }

    public void Tick()
    {
        if (_isPause)
            return;

        float playerAngleY = _playerState.EulerAngles.y;
        float cameraAngleY = _cameraTransform.eulerAngles.y;

        HorizontalMovementResult horizontalResult = _horizontalCalculator
            .Calculate(_playerState.WalkInput.CurrentValue, playerAngleY, cameraAngleY,
            _playerState.IsWalking.CurrentValue, _playerState.IsRunning.CurrentValue);

        _velocity.x = horizontalResult.Velocity.x;
        _velocity.z = horizontalResult.Velocity.z;

        if (_playerState.IsGrounded.CurrentValue && _velocity.y < 0)
            _velocity.y = _playerSettings.StickToGroundForce;
        else
            _velocity.y -= _playerSettings.GravityForce * Time.deltaTime;

        _frameMotion.Value = _velocity * Time.deltaTime;
        _eulerAngles.Value = new Vector3(0, horizontalResult.AngleY, 0);
    }

    public void Dispose() => _compositeDisposable.Dispose();

    public void SetPause(bool value) => _isPause = value;
}
