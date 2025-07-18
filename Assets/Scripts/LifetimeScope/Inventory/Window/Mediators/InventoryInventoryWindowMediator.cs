using R3;

public class InventoryInventoryWindowMediator : Mediator
{
    private readonly Inventory _inventory;
    private readonly InventoryWindow _inventoryWindow;

    public InventoryInventoryWindowMediator(Inventory inventory, InventoryWindow inventoryWindow)
    {
        _inventory = inventory;
        _inventoryWindow = inventoryWindow;
    }

    public override void Initialize()
    {
        _inventoryWindow.IsOpen
            .Where(isOpen => !isOpen)
            .Subscribe(_ => _inventory.EndDrag())
            .AddTo(CompositeDisposable);
    }
}
