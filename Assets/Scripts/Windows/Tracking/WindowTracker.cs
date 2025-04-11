using R3;

public class WindowTracker
{
    private readonly ReactiveProperty<bool> _isOpen = new(false);
    private readonly ReactiveProperty<WindowSwitch> _currentWindow = new(null);

    public ReadOnlyReactiveProperty<bool> IsOpen => _isOpen;

    public bool TryOpenWindow(WindowSwitch windowSwitch)
    {
        if (_isOpen.Value)
            return false;

        _currentWindow.Value = windowSwitch;
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
