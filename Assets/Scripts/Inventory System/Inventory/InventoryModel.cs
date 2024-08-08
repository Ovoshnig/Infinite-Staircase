using System.Linq;

public class InventoryModel
{
    private readonly SlotModel[] _slotModels;

    public InventoryModel(SlotModel[] slots) => _slotModels = slots;

    public SlotData[] SaveSlots() =>
        _slotModels.Select(slot => slot.Save()).ToArray();

    public void LoadSlots(SlotData[] slotDataArray, ItemDataRepository itemDataRepository)
    {
        for (int i = 0; i < _slotModels.Length; i++)
            _slotModels[i].Load(slotDataArray[i], itemDataRepository);
    }
}
