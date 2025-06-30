using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;

public class ItemGenerator : MonoBehaviour
{
    private InventoryView _inventoryView;
    private ItemDataLoader _itemDataLoader;

    [Inject]
    public void Construct(InventoryView inventoryView, ItemDataLoader itemDataLoader)
    {
        _inventoryView = inventoryView;
        _itemDataLoader = itemDataLoader;
    }

    private async void OnTriggerEnter(Collider other)
    {
        ItemModel itemModel = await GenerateRandomItemAsync();
        _inventoryView.TryAddItem(itemModel);
    }

    private async UniTask<ItemModel> GenerateRandomItemAsync()
    {
        ItemData itemDataSO = await _itemDataLoader.GetRandomItemDataAsync();
        ItemModel itemModel = new(itemDataSO.name, itemDataSO.Icon);
        return itemModel;
    }
}
