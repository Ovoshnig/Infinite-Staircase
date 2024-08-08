using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataRepository", menuName = "Scriptable Objects/ItemDataRepository")]
public class ItemDataRepository : ScriptableObject
{
    [SerializeField] private List<ItemDataSO> _items;

    public ItemDataSO GetItemDataByName(string name)
    {
        _items ??= Resources.LoadAll<ItemDataSO>(ResourcesConstants.ItemsPath).ToList();

        return _items.Find(item => item.Name == name);
    }
}
