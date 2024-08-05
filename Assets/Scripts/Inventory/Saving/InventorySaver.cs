using UnityEngine;
using Zenject;

public class InventorySaver : MonoBehaviour
{
    private const string InventoryKey = "Inventory";

    private SlotView[] _slots;
    private SaveSaver _saveSaver;
    private ItemDataRepository _itemDataRepository;

    [Inject]
    private void Construct(SaveSaver saveSaver, ItemDataRepository itemDataRepository)
    {
        _saveSaver = saveSaver;
        _itemDataRepository = itemDataRepository;
    }

    private void Awake() => _slots = GetComponentsInChildren<SlotView>();

    private void Start()
    {
        SlotData[] slotDataArray = _saveSaver.LoadData(InventoryKey, new SlotData[_slots.Length]);

        for (int i = 0; i < _slots.Length; i++)
            _slots[i].Load(slotDataArray[i], _itemDataRepository);
    }

    private void OnDestroy()
    {
        SlotData[] slotDataArray = new SlotData[_slots.Length];

        for (int i = 0; i < _slots.Length; i++)
            slotDataArray[i] = _slots[i].Save();

        _saveSaver.SaveData(InventoryKey, slotDataArray);
    }

    public bool AddItem(ItemModel itemModel)
    {
        foreach (var slot in _slots)
        {
            if (!slot.SlotModel.HasItem)
                slot.PlaceItem(itemModel);

            return true;
        }

        return false;
    }

    public void UseItem(SlotView slotView)
    {
        if (slotView.SlotModel.HasItem)
        {
            // Implement the logic for using the item
            // For example:
            // ItemModel itemModel = slotView.SlotModel.StoredItem;
            // itemModel.Use();

            slotView.TakeItem();
        }
    }
}
