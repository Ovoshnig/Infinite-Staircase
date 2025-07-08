using R3;

public class WindowTracker
{
    private readonly ReactiveProperty<bool> _isOpen = new(false);
    private readonly ReactiveProperty<Window> _currentWindow = new(null);

    public ReadOnlyReactiveProperty<bool> IsOpen => _isOpen;

    public bool TryOpenWindow(Window window)
    {
        if (_isOpen.Value)
            return false;

        _currentWindow.Value = window;
        _isOpen.Value = true;
        return true;
    }

    public bool TryCloseWindow()
    {
        if (!_isOpen.Value)
            return false;

        _currentWindow.Value = null;
        _isOpen.Value = false;
        return true;
    }
}
