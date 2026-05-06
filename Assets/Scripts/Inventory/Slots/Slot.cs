using Cysharp.Threading.Tasks;
using R3;
using System.Threading;
using UnityEngine;

public class Slot
{
    private readonly ReactiveProperty<ItemData> _itemData = new(null);

    public ReadOnlyReactiveProperty<ItemData> ItemData => _itemData;

    public bool HasItem => _itemData.Value != null;
    public bool IsEmpty => _itemData.Value == null;

    public void PlaceItem(ItemData itemData)
    {
        if (itemData == null)
            return;

        if (HasItem)
        {
            Debug.LogError("Cannot place item in a non-empty slot.");
            return;
        }

        _itemData.Value = itemData;
    }

    public ItemData TakeItem()
    {
        if (IsEmpty)
        {
            Debug.LogWarning("Attempted to take an item from an empty slot.");
            return null;
        }

        ItemData takenItem = _itemData.Value;
        _itemData.Value = null;
        return takenItem;
    }

    public void Clear() => _itemData.Value = null;

    public SlotData ToData() => new() { ItemName = _itemData.Value?.Name };

    public async UniTask LoadFromDataAsync(SlotData slotData,
        ItemDefinitionLoader itemDefinitionLoader, CancellationToken token)
    {
        slotData ??= new SlotData();

        if (string.IsNullOrEmpty(slotData.ItemName))
        {
            Clear();
        }
        else
        {
            ItemDefinition itemDefinition = await itemDefinitionLoader
                .GetItemByNameAsync(slotData.ItemName, token);

            if (itemDefinition == null)
            {
                Debug.LogError($"There is no itemDefinition with name {slotData.ItemName}");
                Clear();
            }
            else
            {
                _itemData.Value = new ItemData(itemDefinition.name, itemDefinition.Icon);
            }
        }
    }
}
