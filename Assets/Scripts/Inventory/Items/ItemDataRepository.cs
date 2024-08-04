using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataRepository", menuName = "Scriptable Objects/ItemDataRepository")]
public class ItemDataRepository : ScriptableObject
{
    private const string ItemsPath = "Items";

    private List<ItemDataSO> _items;

    public ItemDataSO GetItemDataByName(string name)
    {
        _items ??= Resources.LoadAll<ItemDataSO>(ItemsPath).ToList();

        return _items.Find(item => item.Name == name);
    }
}
