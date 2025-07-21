public class KeyListeningTrackerBlockerViewMediatorFactory
    : MediatorViewFactory<KeyListeningTrackerBlockerViewMediator, BlockerView>
{
    private readonly KeyListeningTracker _keyListeningTracker;

    public KeyListeningTrackerBlockerViewMediatorFactory(KeyListeningTracker keyListeningTracker) =>
        _keyListeningTracker = keyListeningTracker;

    protected override KeyListeningTrackerBlockerViewMediator CreateMediatorInstance(BlockerView view) =>
        new(_keyListeningTracker, view);
}
