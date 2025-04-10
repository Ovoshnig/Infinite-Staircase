using R3;
using System;
using VContainer.Unity;

public class WindowTrackerPlayerInputMediator : IInitializable, IDisposable
{
    private readonly WindowTracker _windowTracker;
    private readonly InputActions _inputActions;
    private readonly CompositeDisposable _compositeDisposable = new();

    public WindowTrackerPlayerInputMediator(WindowTracker windowTracker, InputActions inputActions)
    {
        _windowTracker = windowTracker;
        _inputActions = inputActions;
    }

    public void Initialize()
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
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
