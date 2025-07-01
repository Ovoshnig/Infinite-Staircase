using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;

public class ItemGenerator : MonoBehaviour
{
    private Inventory _inventory;
    private ItemDefinitionLoader _itemDefinitionLoader;

    [Inject]
    public void Construct(Inventory inventory, ItemDefinitionLoader itemDefinitionLoader)
    {
        _inventory = inventory;
        _itemDefinitionLoader = itemDefinitionLoader;
    }

    private async void OnTriggerEnter(Collider other)
    {
        ItemData itemData = await GenerateRandomItemAsync();
        _inventory.TryAddItem(itemData);
    }

    private async UniTask<ItemData> GenerateRandomItemAsync()
    {
        ItemDefinition itemDataSO = await _itemDefinitionLoader.GetRandomItemAsync();
        ItemData itemData = new(itemDataSO.name, itemDataSO.Icon);
        return itemData;
    }
}
