using R3;
using System;
using VContainer.Unity;

public class WindowTrackerPlayerInputMediator : IInitializable, IDisposable
{
    private readonly WindowTracker _windowTracker;
    private readonly PlayerInput _playerInput;
    private readonly CompositeDisposable _compositeDisposable = new();

    public WindowTrackerPlayerInputMediator(WindowTracker windowTracker, PlayerInput playerInput)
    {
        _windowTracker = windowTracker;
        _playerInput = playerInput;
    }

    public void Initialize()
    {
        PlayerInput.PlayerActions playerActions = _playerInput.Player;

        _windowTracker.IsOpen
            .Subscribe(value =>
            {
                if (value)
                    playerActions.Disable();
                else
                    playerActions.Enable();
            })
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
