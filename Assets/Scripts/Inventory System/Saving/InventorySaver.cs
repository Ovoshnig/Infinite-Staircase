using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using VContainer.Unity;

public class InventorySaver : IInitializable, IDisposable
{
    private readonly Inventory _inventory;
    private readonly SaveStorage _saveStorage;
    private readonly ItemDefinitionLoader _itemDefinitionLoader;
    private readonly CancellationTokenSource _cts = new();

    public InventorySaver(Inventory inventory, SaveStorage saveStorage, 
        ItemDefinitionLoader itemDefinitionLoader)
    {
        _inventory = inventory;
        _saveStorage = saveStorage;
        _itemDefinitionLoader = itemDefinitionLoader;
    }

    public async void Initialize()
    {
        try
        {
            await LoadInventoryAsync(_cts.Token);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();

        SaveInventory();
    }

    public async UniTask LoadInventoryAsync(CancellationToken token)
    {
        SlotData[] defaultSlotArray = _inventory.ToData();
        SlotData[] slotDataArray = _saveStorage.Get(SaveConstants.InventoryKey, defaultSlotArray);
        await _inventory.LoadFromDataAsync(slotDataArray, _itemDefinitionLoader, token);
    }

    public void SaveInventory()
    {
        SlotData[] slotDataArray = _inventory.ToData();
        _saveStorage.Set(SaveConstants.InventoryKey, slotDataArray);
    }
}
