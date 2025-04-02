using R3;

public class KeyListeningTracker
{
    private readonly ReactiveProperty<bool> _isListening = new(false);

    public ReadOnlyReactiveProperty<bool> IsListening => _isListening;

    public bool TryStartListening()
    {
        if (_isListening.Value)
            return false;

        _isListening.Value = true;

        return true;
    }

    public void StopListening() => _isListening.Value = false;
}
