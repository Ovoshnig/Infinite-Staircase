using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Inventory
{
    public event Action<ItemData> DragStarted;
    public event Action DragEnded;

    private readonly Slot[] _slots;

    public Slot DraggingSlot { get; private set; }
    public Slot HoveredSlot { get; private set; }
    public bool IsDragging => DraggingSlot != null;

    public Inventory(uint slotCount)
    {
        _slots = new Slot[slotCount];

        for (int i = 0; i < slotCount; i++)
            _slots[i] = new Slot();
    }

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
        if (slot.HasItem)
        {
            Debug.Log($"Removing item: {slot.ItemData.CurrentValue.Name}");
            slot.TakeItem();
            return true;
        }

        return false;
    }

    public void SelectSlot(Slot slot) => HoveredSlot = slot;

    public void DeselectSlot(Slot slot)
    {
        if (HoveredSlot == slot)
            HoveredSlot = null;
    }

    public void BeginDrag(Slot slot)
    {
        if (slot.IsEmpty || IsDragging) 
            return;

        DraggingSlot = slot;
        DragStarted?.Invoke(DraggingSlot.ItemData.CurrentValue);
    }

    public void Drop()
    {
        if (!IsDragging) 
            return;

        ItemData draggedItem = DraggingSlot.TakeItem();

        if (HoveredSlot != null && HoveredSlot != DraggingSlot)
        {
            if (HoveredSlot.HasItem)
            {
                ItemData itemToSwap = HoveredSlot.TakeItem();
                DraggingSlot.PlaceItem(itemToSwap);
            }

            HoveredSlot.PlaceItem(draggedItem);
        }
        else
        {
            DraggingSlot.PlaceItem(draggedItem);
        }

        DraggingSlot = null;
        DragEnded?.Invoke();
    }

    public SlotData[] ToData() => _slots.Select(slot => slot.ToData()).ToArray();

    public async UniTask LoadFromDataAsync(SlotData[] slotDataArray, ItemDefinitionLoader itemDefinitionLoader)
    {
        int count = Mathf.Min(_slots.Length, slotDataArray.Length);

        for (int i = 0; i < count; i++)
            await _slots[i].LoadFromDataAsync(slotDataArray[i], itemDefinitionLoader);
    }
}
