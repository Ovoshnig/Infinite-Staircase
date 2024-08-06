using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _playerInput = new PlayerInput();
        _playerInput.Player.Move.performed += OnMovePerformed;
        _playerInput.Player.Run.performed += OnRunPerformed;
        _playerInput.Player.Jump.performed += OnJumpPerformed;
        _playerInput.Player.Move.canceled += OnMoveCanceled;
        _playerInput.Player.Run.canceled += OnRunCanceled;
        _playerInput.Player.Jump.canceled += OnJumpCancelled;
    }

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();

    private void OnMovePerformed(InputAction.CallbackContext _) => 
        _animator.SetBool(PlayerAnimatorConstants.IsWalking, true);

    private void OnRunPerformed(InputAction.CallbackContext _) => 
        _animator.SetBool(PlayerAnimatorConstants.IsRunning, true);

    private void OnJumpPerformed(InputAction.CallbackContext _) => 
        _animator.SetBool(PlayerAnimatorConstants.IsJumping, true);

    private void OnMoveCanceled(InputAction.CallbackContext _) => 
        _animator.SetBool(PlayerAnimatorConstants.IsWalking, false);

    private void OnRunCanceled(InputAction.CallbackContext _) => 
        _animator.SetBool(PlayerAnimatorConstants.IsRunning, false);

    private void OnJumpCancelled(InputAction.CallbackContext _) => 
        _animator.SetBool(PlayerAnimatorConstants.IsJumping, false);
}
