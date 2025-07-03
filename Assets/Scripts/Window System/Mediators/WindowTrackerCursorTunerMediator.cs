using R3;
using System;
using VContainer.Unity;

public class WindowTrackerCursorTunerMediator : IInitializable, IDisposable
{
    private readonly WindowTracker _windowTracker;
    private readonly CursorTuner _cursorTuner;
    private readonly CompositeDisposable _compositeDisposable = new();

    public WindowTrackerCursorTunerMediator(WindowTracker windowTracker, CursorTuner cursorTuner)
    {
        _windowTracker = windowTracker;
        _cursorTuner = cursorTuner;
    }

    public void Initialize()
    {
        _windowTracker.IsOpen
            .Subscribe(value =>
            {
                if (value)
                    _cursorTuner.ShowCursor();
                else
                    _cursorTuner.HideCursor();
            })
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
