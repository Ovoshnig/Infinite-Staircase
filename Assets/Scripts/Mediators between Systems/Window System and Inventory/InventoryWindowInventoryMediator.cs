using R3;

public class InventoryWindowInventoryMediator : Mediator
{
    private readonly InventoryWindow _inventoryWindow;
    private readonly Inventory _inventory;

    public InventoryWindowInventoryMediator(InventoryWindow inventoryWindow, Inventory inventory)
    {
        _inventoryWindow = inventoryWindow;
        _inventory = inventory;
    }

    public override void Initialize()
    {
        _inventoryWindow.IsOpen
            .Where(value => !value)
            .Subscribe(_ => _inventory.EndDrag())
            .AddTo(CompositeDisposable);
    }
}
