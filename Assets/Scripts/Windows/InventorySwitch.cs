using R3;

public class InventorySwitch : WindowSwitch
{
    protected override void InitializeInput()
    {
        Disposable = InputHandler.InventorySwitchPressed
            .Where(value => value)
            .Subscribe(_ => Switch());
    }
}
