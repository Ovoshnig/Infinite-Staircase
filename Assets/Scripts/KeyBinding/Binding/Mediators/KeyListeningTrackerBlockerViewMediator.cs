using R3;

public class KeyListeningTrackerBlockerViewMediator : Mediator
{
    private readonly ButtonListener _buttonListener;
    private readonly BlockerView _blockerView;

    public KeyListeningTrackerBlockerViewMediator(ButtonListener buttonListener,
        BlockerView blockerView)
    {
        _buttonListener = buttonListener;
        _blockerView = blockerView;
    }

    public override void Initialize()
    {
        _buttonListener.IsListening
            .Select(isListening => isListening
                ? Observable.Return(true)
                : Observable.Return(false).DelayFrame(2))
            .Switch()
            .Subscribe(_blockerView.SetActive)
            .AddTo(CompositeDisposable);
    }
}
