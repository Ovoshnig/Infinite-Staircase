public class KeyListeningTrackerBlockerViewMediatorFactory
    : MediatorViewFactory<KeyListeningTrackerBlockerViewMediator, BlockerView>
{
    private readonly ButtonListener _buttonListener;

    public KeyListeningTrackerBlockerViewMediatorFactory(ButtonListener buttonListener) =>
        _buttonListener = buttonListener;

    protected override KeyListeningTrackerBlockerViewMediator CreateMediatorInstance(BlockerView view) =>
        new(_buttonListener, view);
}
