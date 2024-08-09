using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerState : MonoBehaviour
{
    private PlayerInputHandler _inputHandler;
    private CharacterController _characterController;

    public event Action WalkStarted;
    public event Action RunStarted;
    public event Action JumpStarted;
    public event Action WalkEnded;
    public event Action RunEnded;
    public event Action JumpEnded;
    public event Action WindowClosed;

    public Vector2 WalkInput => _inputHandler.WalkInput;
    public Vector2 LookInput => _inputHandler.LookInput;
    public bool IsWalking => _characterController.isGrounded && 
        _inputHandler.IsWalkPressed && !_inputHandler.IsRunPressed;
    public bool IsRunning => _characterController.isGrounded &&
        _inputHandler.IsWalkPressed && _inputHandler.IsRunPressed;
    public bool IsInAir => !_characterController.isGrounded;
    public bool IsLooking => _inputHandler.IsLookPressed;

    [Inject]
    private void Construct(PlayerInputHandler inputHandler) => _inputHandler = inputHandler;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _inputHandler.WalkPerformed += OnWalkPerformed;
        _inputHandler.RunPerformed += OnRunPerformed;
        _inputHandler.JumpPerformed += OnJumpPerformed;
        _inputHandler.WalkCanceled += OnWalkCanceled;
        _inputHandler.RunCanceled += OnRunCanceled;
        _inputHandler.WindowClosed += OnWindowClosed;
    }

    private void OnDestroy()
    {
        _inputHandler.WalkPerformed -= OnWalkPerformed;
        _inputHandler.RunPerformed -= OnRunPerformed;
        _inputHandler.JumpPerformed -= OnJumpPerformed;
        _inputHandler.WalkCanceled -= OnWalkCanceled;
        _inputHandler.RunCanceled -= OnRunCanceled;
        _inputHandler.WindowClosed -= OnWindowClosed;
    }

    private void OnWalkPerformed()
    {
        if (IsWalking)
            WalkStarted?.Invoke();
    }

    private void OnRunPerformed()
    {
        if (IsRunning)
            RunStarted?.Invoke();
    }

    private async void OnJumpPerformed()
    {
        if (IsInAir)
            return;

        JumpStarted?.Invoke();
        await UniTask.WaitWhile(() => _characterController.isGrounded);
        await UniTask.WaitUntil(() => _characterController.isGrounded);
        JumpEnded?.Invoke();
    }

    private void OnWalkCanceled() => WalkEnded?.Invoke();

    private void OnRunCanceled() => RunEnded?.Invoke();

    private void OnWindowClosed() => WindowClosed?.Invoke();
}
