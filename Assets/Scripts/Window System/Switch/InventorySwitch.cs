using R3;

public class InventorySwitch : WindowSwitch
{
    public InventorySwitch(WindowInputHandler windowInputHandler, WindowTracker windowTracker) 
        : base(windowInputHandler, windowTracker)
    {
    }

    protected override ReadOnlyReactiveProperty<bool> WindowSwitchPressed => 
        WindowInputHandler.InventorySwitchPressed;
}
