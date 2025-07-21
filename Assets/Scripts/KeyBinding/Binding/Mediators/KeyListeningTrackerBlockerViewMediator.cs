using R3;

public class KeyListeningTrackerBlockerViewMediator : Mediator
{
    private readonly KeyListeningTracker _keyListeningTracker;
    private readonly BlockerView _blockerView;

    public KeyListeningTrackerBlockerViewMediator(KeyListeningTracker keyListeningTracker,
        BlockerView blockerView)
    {
        _keyListeningTracker = keyListeningTracker;
        _blockerView = blockerView;
    }

    public override void Initialize()
    {
        _keyListeningTracker.IsListening
            .Subscribe(_blockerView.SetActive)
            .AddTo(CompositeDisposable);
    }
}
