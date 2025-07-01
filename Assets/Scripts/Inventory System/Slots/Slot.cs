using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Slot
{
    public event Action<ItemData> OnItemChanged;

    private ItemData _itemData;

    public ItemData ItemData
    {
        get => _itemData;
        private set
        {
            if (_itemData != value)
            {
                _itemData = value;
                OnItemChanged?.Invoke(_itemData);
            }
        }
    }

    public bool HasItem => ItemData != null;
    public bool IsEmpty => ItemData == null;

    public void PlaceItem(ItemData itemData)
    {
        if (itemData == null) 
            return;

        if (HasItem)
        {
            Debug.LogError("Cannot place item in a non-empty slot.");
            return;
        }

        ItemData = itemData;
    }

    public ItemData TakeItem()
    {
        if (IsEmpty)
        {
            Debug.LogWarning("Attempted to take an item from an empty slot.");
            return null;
        }

        ItemData takenItem = ItemData;
        ItemData = null;
        return takenItem;
    }

    public void Clear() => ItemData = null;

    public SlotData ToData() => new() { ItemName = ItemData?.Name };

    public async UniTask LoadFromDataAsync(SlotData slotData, ItemDefinitionLoader itemDefinitionLoader)
    {
        slotData ??= new SlotData();

        if (string.IsNullOrEmpty(slotData.ItemName))
        {
            Clear();
        }
        else
        {
            ItemDefinition itemDefinition = await itemDefinitionLoader.GetItemByNameAsync(slotData.ItemName);

            if (itemDefinition == null)
            {
                Debug.LogError($"There is no itemDefinition with name {slotData.ItemName}");
                Clear();
            }
            else
            {
                ItemData = new ItemData(itemDefinition.name, itemDefinition.Icon);
            }
        }
    }
}
