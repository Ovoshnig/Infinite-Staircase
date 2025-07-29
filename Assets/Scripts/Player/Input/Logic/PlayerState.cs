using R3;
using System;
using UnityEngine;
using VContainer.Unity;

public class PlayerState : IInitializable, IDisposable
{
    private readonly PlayerInputHandler _playerInputHandler;
    private readonly CharacterController _characterController;
    private readonly Subject<Unit> _jumped = new();
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
    public Observable<Unit> Jumped => _jumped;
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
        Running = Walking
            .CombineLatest(
                _playerInputHandler.RunPressed,
                (isWalking, isRunning) =>
                    isWalking && isRunning
            )
            .ToReadOnlyReactiveProperty()
            .AddTo(_compositeDisposable);
        _playerInputHandler.JumpPressed
            .Where(isPressed => isPressed && Grounded.CurrentValue)
            .Subscribe(_ => _jumped.OnNext(Unit.Default))
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable.Dispose();
}
