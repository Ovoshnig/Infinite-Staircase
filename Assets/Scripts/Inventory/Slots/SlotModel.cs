public class SlotModel
{
    public bool HasItem { get; private set; }
    public ItemModel StoredItem { get; private set; }

    public void PlaceItem(ItemModel item)
    {
        StoredItem = item;
        HasItem = true;
    }

    public ItemModel TakeItem()
    {
        ItemModel item = StoredItem;
        StoredItem = null;
        HasItem = false;
        return item;
    }

    public SlotData Save()
    {
        return new SlotData
        {
            HasItem = HasItem,
            ItemName = StoredItem?.Name
        };
    }

    public void Load(SlotData data, ItemDataRepository itemDataRepository)
    {
        data ??= new SlotData
        {
            HasItem = HasItem,
            ItemName = default
        };

        if (data.HasItem && data.ItemName != null)
        {
            ItemDataSO itemData = itemDataRepository.GetItemDataByName(data.ItemName);

            if (itemData != null)
            {
                StoredItem = new ItemModel(itemData.Name, itemData.Icon);
                HasItem = true;
            }
        }
        else
        {
            StoredItem = null;
            HasItem = false;
        }
    }
}
