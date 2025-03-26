using Random = System.Random;
using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;

public class ItemGenerator : MonoBehaviour
{
    private readonly string[] _itemNames = new string[] { "Gear", "Music", "Play" };
    private InventoryView _inventoryView;
    private ItemDataRepository _itemDataRepository;
    private Random _random;

    private void Awake() => _random = new Random();

    [Inject]
    public void Construct(InventoryView inventoryView, ItemDataRepository itemDataRepository)
    {
        _inventoryView = inventoryView;
        _itemDataRepository = itemDataRepository;
    }

    private async void OnTriggerEnter(Collider other)
    {
        ItemModel itemModel = await GenerateRandomItemAsync();
        _inventoryView.TryAddItem(itemModel);
    }

    private async UniTask<ItemModel> GenerateRandomItemAsync()
    {
        int index = _random.Next(0, _itemNames.Length);
        string name = _itemNames[index];
        ItemDataSO itemDataSO = await _itemDataRepository.GetItemDataByNameAsync(name);
        ItemModel itemModel = new(name, itemDataSO.Icon);

        return itemModel;
    }
}
