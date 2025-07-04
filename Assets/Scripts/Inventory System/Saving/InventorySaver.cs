using Cysharp.Threading.Tasks;
using System.Threading;

public class InventorySaver
{
    private readonly SaveStorage _saveStorage;
    private readonly ItemDefinitionLoader _itemDefinitionLoader;

    public InventorySaver(SaveStorage saveStorage, ItemDefinitionLoader itemDefinitionLoader)
    {
        _saveStorage = saveStorage;
        _itemDefinitionLoader = itemDefinitionLoader;
    }

    public async UniTask LoadInventoryAsync(Inventory inventory, CancellationToken token)
    {
        SlotData[] defaultSlotArray = inventory.ToData();
        SlotData[] slotDataArray = _saveStorage.Get(SaveConstants.InventoryKey, defaultSlotArray);
        await inventory.LoadFromDataAsync(slotDataArray, _itemDefinitionLoader, token);
    }

    public void SaveInventory(Inventory inventory)
    {
        SlotData[] slotDataArray = inventory.ToData();
        _saveStorage.Set(SaveConstants.InventoryKey, slotDataArray);
    }
}
