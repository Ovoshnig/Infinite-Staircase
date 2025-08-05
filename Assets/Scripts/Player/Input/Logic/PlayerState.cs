using R3;
using System;
using UnityEngine;
using VContainer.Unity;

public class PlayerState : IInitializable, IDisposable
{
    private readonly PlayerInputHandler _playerInputHandler;
    private readonly CharacterController _characterController;
    private readonly CompositeDisposable _compositeDisposable = new();

    public PlayerState(PlayerInputHandler playerInputHandler,
        CharacterController characterController)
    {
        _playerInputHandler = playerInputHandler;
        _characterController = characterController;
    }

    public ReadOnlyReactiveProperty<Vector2> WalkInput => _playerInputHandler.WalkInput;
    public ReadOnlyReactiveProperty<Vector2> LookInput => _playerInputHandler.LookInput;
    public ReadOnlyReactiveProperty<bool> Walking { get; private set; }
    public ReadOnlyReactiveProperty<bool> Looking { get; private set; }
    public ReadOnlyReactiveProperty<bool> Running { get; private set; }
    public ReadOnlyReactiveProperty<bool> Grounded { get; private set; }
    public Observable<Unit> Jumped { get; private set; }
    public Vector3 EulerAngles => _characterController.transform.eulerAngles;

    public void Initialize()
    {
        Walking = WalkInput
            .Select(value => value != Vector2.zero)
            .ToReadOnlyReactiveProperty()
            .AddTo(_compositeDisposable);
        Looking = LookInput
            .Select(value => value != Vector2.zero)
            .ToReadOnlyReactiveProperty()
            .AddTo(_compositeDisposable);

        Grounded = Observable
            .EveryValueChanged(this, p => _characterController.isGrounded)
            .ToReadOnlyReactiveProperty()
            .AddTo(_compositeDisposable);
        Running = new ReactiveProperty<bool>(false);
        Running = Observable
            .CombineLatest(
                Walking,
                _playerInputHandler.RunPressed,
                Grounded,
                (isWalking, isRunningPressed, isGrounded) =>
                    isWalking && isRunningPressed && (isGrounded || Running.CurrentValue))
            .ToReadOnlyReactiveProperty()
            .AddTo(_compositeDisposable);
        Jumped = _playerInputHandler.JumpPressed
            .Where(isPressed => isPressed && Grounded.CurrentValue)
            .Select(_ => Unit.Default);
    }

    public void Dispose() => _compositeDisposable.Dispose();
}
