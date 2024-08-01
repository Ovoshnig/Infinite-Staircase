using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private const string IsWalking = "isWalking";
    private const string IsRunning = "isRunning";
    private const string IsJumping = "isJumping";

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

    private void OnMovePerformed(InputAction.CallbackContext _) => _animator.SetBool(IsWalking, true);

    private void OnRunPerformed(InputAction.CallbackContext _) => _animator.SetBool(IsRunning, true);

    private void OnJumpPerformed(InputAction.CallbackContext _) => _animator.SetBool(IsJumping, true);

    private void OnMoveCanceled(InputAction.CallbackContext _) => _animator.SetBool(IsWalking, false);

    private void OnRunCanceled(InputAction.CallbackContext _) => _animator.SetBool(IsRunning, false);

    private void OnJumpCancelled(InputAction.CallbackContext _) => _animator.SetBool(IsJumping, false);
}
