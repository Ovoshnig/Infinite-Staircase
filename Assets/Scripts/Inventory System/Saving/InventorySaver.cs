using System.Linq;
using Zenject;

public class InventorySaver
{
    private readonly SaveSaver _saveSaver;
    private readonly ItemDataRepository _itemDataRepository;

    [Inject]
    public InventorySaver(SaveSaver saveSaver, ItemDataRepository itemDataRepository)
    {
        _saveSaver = saveSaver;
        _itemDataRepository = itemDataRepository;
    }

    public void LoadSlots(SlotView[] slotViews)
    {
        SlotData[] defaultSlotArray = slotViews.Select(_ => new SlotData()).ToArray();
        SlotData[] slotDataArray = _saveSaver.LoadData(SaveConstants.InventoryKey, defaultSlotArray);

        for (int i = 0; i < slotViews.Length; i++)
            slotViews[i].Load(slotDataArray[i], _itemDataRepository);
    }

    public void SaveSlots(SlotView[] slotViews, SlotData[] slotDataArray)
    {
        for (int i = 0; i < slotViews.Length; i++)
            slotDataArray[i] = slotViews[i].Save();
        _saveSaver.SaveData(SaveConstants.InventoryKey, slotDataArray);
    }
}
