using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = System.Random;

public class ItemDefinitionLoader
{
    private readonly Random random = new();
    private List<ItemDefinition> _items = null;

    public async UniTask<ItemDefinition> GetItemByNameAsync(string name)
    {
        List<ItemDefinition> items = await GetItemsAsync();
        return items.Find(item => item.name == name);
    }

    public async UniTask<ItemDefinition> GetRandomItemAsync()
    {
        List<ItemDefinition> items = await GetItemsAsync();
        int index = random.Next(0, items.Count);
        return items[index];
    }

    private async UniTask<List<ItemDefinition>> GetItemsAsync()
    {
        if (_items == null)
        {
            AsyncOperationHandle<IList<ItemDefinition>> handler = Addressables
                .LoadAssetsAsync<ItemDefinition>("item");
            await handler.ToUniTask();

            if (handler.Status == AsyncOperationStatus.Succeeded)
                _items = handler.Result.ToList();
        }

        return _items;
    }
}
