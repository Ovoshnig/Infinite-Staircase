using UnityEngine;
using VContainer;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoverView : MonoBehaviour
{
    private PlayerHorizontalMover _playerHorizontalMover;
    private PlayerVerticalMover _playerVerticalMover;
    private CharacterController _characterController;

    [Inject]
    public void Construct(PlayerHorizontalMover playerHorizontalMover,
        PlayerVerticalMover playerVerticalMover)
    {
        _playerHorizontalMover = playerHorizontalMover;
        _playerVerticalMover = playerVerticalMover;
    }

    private void Awake() => _characterController = GetComponent<CharacterController>();

    private void Update()
    {
        transform.eulerAngles = _playerHorizontalMover.EulerAngles;

        Vector3 horizontalMovement = _playerHorizontalMover.CalculateMovementVector();
        Vector3 verticalMovement = _playerVerticalMover.CalculateMovementVector();
        
        _characterController.Move(horizontalMovement);
        _characterController.Move(verticalMovement);
    }
}
