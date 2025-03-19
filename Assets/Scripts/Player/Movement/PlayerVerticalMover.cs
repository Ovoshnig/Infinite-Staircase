using R3;
using System;
using UnityEngine;
using VContainer.Unity;

[RequireComponent(typeof(CharacterController))]
public class PlayerVerticalMover : IInitializable, IDisposable
{
    private readonly PlayerSettings _playerSettings;
    private readonly CompositeDisposable _compositeDisposable = new();
    private readonly PlayerState _playerState;
    private Vector3 _movement;

    public PlayerVerticalMover(PlayerSettings playerSettings, PlayerState playerState)
    {
        _playerSettings = playerSettings;
        _playerState = playerState;
    }

    public void Initialize()
    {
        var jumpDisposable = _playerState.IsJumping
            .Where(value => value)
            .Subscribe(_ => _movement.y = _playerSettings.JumpForce)
            .AddTo(_compositeDisposable);

        var groundEnterDisposable = _playerState.IsGrounded
            .Where(value => value)
            .Subscribe(_ => _movement.y = -_playerSettings.GravityForce)
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();

    public Vector3 CalculateMovementVector()
    {
        if (!_playerState.IsGrounded.CurrentValue)
            _movement.y -= _playerSettings.GravityForce * Time.deltaTime;

        Vector3 multipliedMovement = _movement * Time.deltaTime;

        return multipliedMovement;
    }
}
