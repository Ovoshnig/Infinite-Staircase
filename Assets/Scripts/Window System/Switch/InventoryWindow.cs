using R3;

public class InventoryWindow : Window
{
    public InventoryWindow(WindowInputHandler windowInputHandler, WindowTracker windowTracker) 
        : base(windowInputHandler, windowTracker)
    {
    }

    protected override ReadOnlyReactiveProperty<bool> WindowSwitchPressed => 
        WindowInputHandler.InventorySwitchPressed;
}
