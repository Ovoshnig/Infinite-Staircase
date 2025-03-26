using Cysharp.Threading.Tasks;
using System.Linq;

public class InventorySaver
{
    private readonly SaveStorage _saveStorage;
    private readonly ItemDataRepository _itemDataRepository;

    public InventorySaver(SaveStorage saveStorage, ItemDataRepository itemDataRepository)
    {
        _saveStorage = saveStorage;
        _itemDataRepository = itemDataRepository;
    }

    public async UniTask LoadSlotsAsync(SlotView[] slotViews)
    {
        SlotData[] defaultSlotArray = slotViews.Select(_ => new SlotData()).ToArray();
        SlotData[] slotDataArray = _saveStorage.Get(SaveConstants.InventoryKey, defaultSlotArray);

        for (int i = 0; i < slotViews.Length; i++)
            await slotViews[i].LoadAsync(slotDataArray[i], _itemDataRepository);
    }

    public void SaveSlots(SlotView[] slotViews, SlotData[] slotDataArray)
    {
        for (int i = 0; i < slotViews.Length; i++)
            slotDataArray[i] = slotViews[i].Save();

        _saveStorage.Set(SaveConstants.InventoryKey, slotDataArray);
    }
}
