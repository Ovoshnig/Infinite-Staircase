using Random = System.Random;
using UnityEngine;
using Zenject;

public class ItemGenerator : MonoBehaviour
{
    private readonly string[] _itemNames = new string[] { "Gear", "Music", "Play" };
    private InventorySaver _inventorySaver;
    private ItemDataRepository _itemDataRepository;
    private Random _random;

    private void Awake() => _random = new Random();

    [Inject]
    private void Construct(InventorySaver inventorySaver, ItemDataRepository itemDataRepository)
    {
        _inventorySaver = inventorySaver;
        _itemDataRepository = itemDataRepository;
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemModel itemModel = GenerateRandomItem();
        _inventorySaver.TryAddItem(itemModel);
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
