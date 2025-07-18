using R3;

public class PlayerActionsWindowTrackerMediator : Mediator
{
    private readonly InputActions.PlayerActions _playerActions;
    private readonly WindowTracker _windowTracker;

    public PlayerActionsWindowTrackerMediator(InputActions inputActions, WindowTracker windowTracker)
    {
        _playerActions = inputActions.Player;
        _windowTracker = windowTracker;
    }

    public override void Initialize()
    {
        _windowTracker.IsOpen
            .Subscribe(isOpen =>
            {
                if (isOpen)
                    _playerActions.Disable();
                else
                    _playerActions.Enable();
            })
            .AddTo(CompositeDisposable);
    }
}
