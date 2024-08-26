public interface IWindowSwitch
{
    public bool Open();
    public bool Close();
    public bool IsOpen { get; }
}
