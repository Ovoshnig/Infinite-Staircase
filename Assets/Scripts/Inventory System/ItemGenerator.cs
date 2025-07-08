using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using VContainer;

public class ItemGenerator : MonoBehaviour
{
    private readonly CancellationTokenSource _cts = new();

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
        try
        {
            ItemData itemData = await GenerateRandomItemAsync(_cts.Token);
            _inventory.TryAddItem(itemData);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    private async UniTask<ItemData> GenerateRandomItemAsync(CancellationToken token)
    {
        ItemDefinition itemDataSO = await _itemDefinitionLoader.GetRandomItemAsync(token);
        ItemData itemData = new(itemDataSO.name, itemDataSO.Icon);
        return itemData;
    }
}
