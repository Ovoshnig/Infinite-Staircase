using UnityEngine;
using Zenject;

public class InventoryGrid : MonoBehaviour
{
    private const string InventoryKey = "Inventory";

    private Slot[] _slots;
    private string[] _itemNames;
    private DataSaver _dataSaver;
    
    [Inject]
    private void Construct(DataSaver dataSaver) => _dataSaver = dataSaver;

    private void Awake() => _slots = GetComponentsInChildren<Slot>();

    private void Start()
    {
        _itemNames = _dataSaver.LoadData<string[]>(InventoryKey);

    }
}
