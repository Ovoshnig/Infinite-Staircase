using R3;

public class InventorySwitch : WindowSwitch
{
    protected override void InitializeInput()
    {
        Disposable = WindowInputHandler.InventorySwitchPressed
            .Where(value => value)
            .Subscribe(_ => Switch());
    }
}
