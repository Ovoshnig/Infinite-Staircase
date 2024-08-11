using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerState : MonoBehaviour
{
    private PlayerInputHandler _inputHandler;
    private CharacterController _characterController;
    private bool _wasGrounded;

    public event Action WalkStarted;
    public event Action RunStarted;
    public event Action JumpStarted;
    public event Action GroundLeft;
    public event Action WalkEnded;
    public event Action RunEnded;
    public event Action JumpEnded;
    public event Action GroundEntered;

    public Vector2 WalkInput => _inputHandler.WalkInput;
    public Vector2 LookInput => _inputHandler.LookInput;
    public bool IsWalking => _inputHandler.IsWalkPressed;
    public bool IsRunning => _inputHandler.IsWalkPressed && _inputHandler.IsRunPressed;
    public bool IsInAir => !_characterController.isGrounded;
    public bool IsLooking => _inputHandler.IsLookPressed;

    [Inject]
    private void Construct(PlayerInputHandler inputHandler) => _inputHandler = inputHandler;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _inputHandler.WalkPerformed += OnWalkPerformed;
        _inputHandler.RunPerformed += OnRunPerformed;
        _inputHandler.WalkCanceled += OnWalkCanceled;
        _inputHandler.RunCanceled += OnRunCanceled;
    }

    private void Start() => _wasGrounded = _characterController.isGrounded;

    private void OnDestroy()
    {
        _inputHandler.WalkPerformed -= OnWalkPerformed;
        _inputHandler.RunPerformed -= OnRunPerformed;
        _inputHandler.WalkCanceled -= OnWalkCanceled;
        _inputHandler.RunCanceled -= OnRunCanceled;
    }

    private void Update()
    {
        bool isGrounded = _characterController.isGrounded;

        if (isGrounded && !_wasGrounded)
        {
            JumpEnded?.Invoke();
            GroundEntered?.Invoke();
        }
        else if (!isGrounded && _wasGrounded)
        {
            GroundLeft?.Invoke();
        }
        else if (isGrounded && _wasGrounded && _inputHandler.IsJumpPressed)
        {
            JumpStarted?.Invoke();
        }

        _wasGrounded = isGrounded;
    }

    private void OnWalkPerformed()
    {
        WalkStarted?.Invoke();

        if (IsRunning)
            RunStarted?.Invoke();
    }

    private void OnRunPerformed()
    {
        if (IsRunning)
            RunStarted?.Invoke();
    }

    private void OnWalkCanceled()
    {
        WalkEnded?.Invoke();
        RunEnded?.Invoke();
    }

    private void OnRunCanceled() => RunEnded?.Invoke();
}
