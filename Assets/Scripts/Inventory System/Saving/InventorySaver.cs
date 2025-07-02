using Cysharp.Threading.Tasks;

public class InventorySaver
{
    private readonly SaveStorage _saveStorage;
    private readonly ItemDefinitionLoader _itemDefinitionLoader;

    public InventorySaver(SaveStorage saveStorage, ItemDefinitionLoader itemDefinitionLoader)
    {
        _saveStorage = saveStorage;
        _itemDefinitionLoader = itemDefinitionLoader;
    }

    public async UniTask LoadInventoryAsync(Inventory inventory)
    {
        SlotData[] defaultSlotArray = inventory.ToData();
        SlotData[] slotDataArray = _saveStorage.Get(SaveConstants.InventoryKey, defaultSlotArray);
        await inventory.LoadFromDataAsync(slotDataArray, _itemDefinitionLoader);
    }

    public void SaveInventory(Inventory inventory)
    {
        SlotData[] slotDataArray = inventory.ToData();
        _saveStorage.Set(SaveConstants.InventoryKey, slotDataArray);
    }
}
