using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "ItemDataRepository", menuName = "Scriptable Objects/ItemDataRepository")]
public class ItemDataRepository : ScriptableObject
{
    [SerializeField] private List<ItemDataSO> _items;

    public async UniTask<ItemDataSO> GetItemDataByNameAsync(string name)
    {
        if (_items == null)
        {
            AsyncOperationHandle<IList<ItemDataSO>> handler = Addressables.LoadAssetsAsync<ItemDataSO>("item");
            await handler.ToUniTask();

            if (handler.Status == AsyncOperationStatus.Succeeded)
                _items = handler.Result.ToList();
        }

        return _items.Find(item => item.Name == name);
    }
}
