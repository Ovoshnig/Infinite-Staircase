using R3;

public class WindowTrackerPlayerInputMediator : Mediator
{
    private readonly WindowTracker _windowTracker;
    private readonly InputActions _inputActions;

    public WindowTrackerPlayerInputMediator(WindowTracker windowTracker, InputActions inputActions)
    {
        _windowTracker = windowTracker;
        _inputActions = inputActions;
    }

    public override void Initialize()
    {
        InputActions.PlayerActions playerActions = _inputActions.Player;

        _windowTracker.IsOpen
            .Subscribe(value =>
            {
                if (value)
                    playerActions.Disable();
                else
                    playerActions.Enable();
            })
            .AddTo(CompositeDisposable);
    }
}
