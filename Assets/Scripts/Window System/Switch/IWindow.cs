using R3;

public interface IWindow
{
    public bool TryOpen();
    public bool TryClose();
    public ReadOnlyReactiveProperty<bool> IsOpen { get; }
}
