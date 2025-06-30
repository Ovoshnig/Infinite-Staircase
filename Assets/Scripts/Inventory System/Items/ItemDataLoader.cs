using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = System.Random;

public class ItemDataLoader
{
    private readonly Random random = new();
    private List<ItemData> _items = null;

    public async UniTask<ItemData> GetItemDataByNameAsync(string name)
    {
        List<ItemData> items = await GetItemsAsync();
        return items.Find(item => item.name == name);
    }

    public async UniTask<ItemData> GetRandomItemDataAsync()
    {
        List<ItemData> items = await GetItemsAsync();
        int index = random.Next(0, items.Count);
        return items[index];
    }

    private async UniTask<List<ItemData>> GetItemsAsync()
    {
        if (_items == null)
        {
            AsyncOperationHandle<IList<ItemData>> handler = Addressables.LoadAssetsAsync<ItemData>("item");
            await handler.ToUniTask();

            if (handler.Status == AsyncOperationStatus.Succeeded)
                _items = handler.Result.ToList();
        }

        return _items;
    }
}
