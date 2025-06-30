using Cysharp.Threading.Tasks;
using UnityEngine;

public class SlotModel
{
    public ItemModel ItemModel { get; private set; } = null;
    public bool HasItem => ItemModel != null;

    public void PlaceItem(ItemModel item) => ItemModel = item;

    public ItemModel TakeItem()
    {
        ItemModel item = ItemModel;
        ItemModel = null;

        return item;
    }

    public SlotData Save() => new() { ItemName = ItemModel?.Name };

    public async UniTask LoadAsync(SlotData slotData, ItemDataLoader itemDataLoader)
    {
        slotData ??= new SlotData { ItemName = default };

        if (slotData.ItemName != null)
        {
            ItemData itemData = await itemDataLoader.GetItemDataByNameAsync(slotData.ItemName);

            if (itemData == null)
                Debug.LogError($"Item repository does not contain item with name {slotData.ItemName}");
            else
                ItemModel = new ItemModel(itemData.name, itemData.Icon);
        }
        else
        {
            ItemModel = null;
        }
    }
}
