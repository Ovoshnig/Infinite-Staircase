using R3;

public class InventorySwitch : WindowSwitch
{
    protected override void InitializeInput()
    {
        InputHandler.InventorySwitchPressed
            .Where(value => value)
            .Subscribe(_ => Switch());
    }
}
