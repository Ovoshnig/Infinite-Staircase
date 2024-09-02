using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerHorizontalMover : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _walkSpeed;
    [SerializeField, Min(0f)] private float _runSpeed;

    private PlayerState _playerState;
    private CharacterController _characterController;

    [Inject]
    protected void Construct(PlayerState playerState) => _playerState = playerState;

    protected virtual void Awake() => _characterController = GetComponent<CharacterController>();

    protected virtual void Update() => Move();

    protected virtual void Move()
    {
        if (_playerState.IsWalking.CurrentValue)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            var currentSpeed = _playerState.IsRunning.CurrentValue ? _runSpeed : _walkSpeed;
            var motion = forward * currentSpeed;
            _characterController.Move(motion * Time.deltaTime);
        }
    }
}
