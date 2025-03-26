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

    public async UniTask LoadAsync(SlotData slotData, ItemDataRepository itemDataRepository)
    {
        slotData ??= new SlotData { ItemName = default };

        if (slotData.ItemName != null)
        {
            ItemDataSO itemData = await itemDataRepository.GetItemDataByNameAsync(slotData.ItemName);

            if (itemData == null)
                Debug.LogError($"Item repository does not contain item with name {slotData.ItemName}");
            else
                ItemModel = new ItemModel(itemData.Name, itemData.Icon);
        }
        else
        {
            ItemModel = null;
        }
    }
}
