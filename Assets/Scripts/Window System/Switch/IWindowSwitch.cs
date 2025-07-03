using R3;

public interface IWindowSwitch
{
    public bool TryOpen();
    public bool TryClose();
    public ReadOnlyReactiveProperty<bool> IsOpen { get; }
}
