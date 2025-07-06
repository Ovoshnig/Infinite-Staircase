using R3;
using System;
using VContainer.Unity;

public class InventoryWindowInventoryMediator : IInitializable, IDisposable
{
    private readonly InventoryWindow _inventoryWindow;
    private readonly Inventory _inventory;
    private readonly CompositeDisposable _compositeDisposable = new();

    public InventoryWindowInventoryMediator(InventoryWindow inventoryWindow, Inventory inventory)
    {
        _inventoryWindow = inventoryWindow;
        _inventory = inventory;
    }

    public void Initialize()
    {
        _inventoryWindow.IsOpen
            .Where(value => !value)
            .Subscribe(_ => _inventory.Drop())
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
