using Cysharp.Threading.Tasks;
using R3;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Inventory
{
    private readonly Slot[] _slots;
    private readonly ReactiveProperty<Slot> _draggingSlot = new(null);
    private readonly ReactiveProperty<Slot> _hoveredSlot = new(null);

    public Inventory(InventorySettings inventorySettings)
    {
        _slots = Enumerable.Range(0, (int)inventorySettings.SlotCount)
            .Select(_ => new Slot())
            .ToArray();
    }

    public ReadOnlyReactiveProperty<Slot> DraggingSlot => _draggingSlot;
    public ReadOnlyReactiveProperty<Slot> HoveredSlot => _hoveredSlot;
    public bool IsDragging => _draggingSlot.CurrentValue != null;

    public Slot GetSlot(int index) => _slots[index];

    public bool TryAddItem(ItemData itemData)
    {
        Slot firstEmptySlot = _slots.FirstOrDefault(s => s.IsEmpty);

        if (firstEmptySlot == null)
            return false;

        firstEmptySlot.PlaceItem(itemData);
        return true;
    }

    public bool TryRemoveItem(Slot slot)
    {
        if (slot.IsEmpty || IsDragging)
            return false;

        Debug.Log($"Removing item: {slot.ItemData.CurrentValue.Name}");
        slot.TakeItem();
        return true;
    }

    public void SelectSlot(Slot slot) => _hoveredSlot.Value = slot;

    public void DeselectSlot(Slot slot)
    {
        if (_hoveredSlot.Value == slot)
            _hoveredSlot.Value = null;
    }

    public void BeginDrag(Slot slot)
    {
        if (slot.IsEmpty || IsDragging)
            return;

        _draggingSlot.Value = slot;
    }

    public void EndDrag()
    {
        if (!IsDragging)
            return;

        ItemData draggedItem = _draggingSlot.Value.TakeItem();

        if (_hoveredSlot.Value != null && _hoveredSlot.Value != _draggingSlot.Value)
        {
            if (_hoveredSlot.Value.HasItem)
            {
                ItemData itemToSwap = _hoveredSlot.Value.TakeItem();
                _draggingSlot.Value.PlaceItem(itemToSwap);
            }

            _hoveredSlot.Value.PlaceItem(draggedItem);
        }
        else
        {
            _draggingSlot.Value.PlaceItem(draggedItem);
        }

        _draggingSlot.Value = null;
    }

    public SlotData[] ToData() => _slots.Select(slot => slot.ToData()).ToArray();

    public async UniTask LoadFromDataAsync(SlotData[] slotDataArray,
        ItemDefinitionLoader itemDefinitionLoader, CancellationToken token)
    {
        int count = Mathf.Min(_slots.Length, slotDataArray.Length);

        for (int i = 0; i < count; i++)
            await _slots[i].LoadFromDataAsync(slotDataArray[i], itemDefinitionLoader, token);
    }
}
