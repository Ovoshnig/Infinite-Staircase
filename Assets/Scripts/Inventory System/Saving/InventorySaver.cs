using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

public class InventorySaver
{
    private readonly SaveStorage _saveStorage;
    private readonly ItemDataLoader _itemDataLoader;

    public InventorySaver(SaveStorage saveStorage, ItemDataLoader itemDataLoader)
    {
        _saveStorage = saveStorage;
        _itemDataLoader = itemDataLoader;
    }

    public async UniTask LoadSlotsAsync(SlotView[] slotViews)
    {
        SlotData[] defaultSlotArray = slotViews.Select(_ => new SlotData()).ToArray();
        SlotData[] slotDataArray = _saveStorage.Get(SaveConstants.InventoryKey, defaultSlotArray);

        int slotCount = Mathf.Min(slotViews.Length, slotDataArray.Length);

        for (int i = 0; i < slotCount; i++)
            await slotViews[i].LoadAsync(slotDataArray[i], _itemDataLoader);
    }

    public void SaveSlots(SlotView[] slotViews, SlotData[] slotDataArray)
    {
        for (int i = 0; i < slotViews.Length; i++)
            slotDataArray[i] = slotViews[i].Save();

        _saveStorage.Set(SaveConstants.InventoryKey, slotDataArray);
    }
}
