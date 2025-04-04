using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerVerticalCalculator
{
    private readonly PlayerSettings _playerSettings;
    private readonly PlayerState _playerState;

    public PlayerVerticalCalculator(PlayerSettings playerSettings, PlayerState playerState)
    {
        _playerSettings = playerSettings;
        _playerState = playerState;
    }

    public void CalculateJumping(ref Vector3 movement) => movement.y = _playerSettings.JumpForce;

    public void CalculateLanding(ref Vector3 movement) => movement.y = -_playerSettings.GravityForce;

    public Vector3 CalculateFalling(ref Vector3 movement)
    {
        if (!_playerState.IsGrounded.CurrentValue)
            movement.y -= _playerSettings.GravityForce * Time.deltaTime;

        Vector3 multipliedMovement = movement * Time.deltaTime;

        return multipliedMovement;
    }
}
