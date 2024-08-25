using Random = System.Random;
using UnityEngine;
using Zenject;

public class ItemGenerator : MonoBehaviour
{
    private readonly string[] _itemNames = new string[] { "Gear", "Music", "Play" };
    private InventoryView _inventoryView;
    private ItemDataRepository _itemDataRepository;
    private Random _random;

    private void Awake() => _random = new Random();

    [Inject]
    private void Construct(InventoryView inventoryView, ItemDataRepository itemDataRepository)
    {
        _inventoryView = inventoryView;
        _itemDataRepository = itemDataRepository;
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemModel itemModel = GenerateRandomItem();
        _inventoryView.TryAddItem(itemModel);
    }

    private ItemModel GenerateRandomItem()
    {
        int index = _random.Next(0, _itemNames.Length);
        string name = _itemNames[index];
        ItemDataSO itemDataSO = _itemDataRepository.GetItemDataByName(name);
        ItemModel itemModel = new(name, itemDataSO.Icon);

        return itemModel;
    }
}
