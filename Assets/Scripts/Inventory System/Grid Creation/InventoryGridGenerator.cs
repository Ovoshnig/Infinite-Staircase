using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class InventoryGridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private InventorySettings _inventorySettings;

    private GridLayoutGroup _gridLayoutGroup;

    private InventorySettings InventorySettings => _inventorySettings;

    [ContextMenu(nameof(PlaceSlots))]
    private void PlaceSlots()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();

        while (transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);

        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = (int)InventorySettings.ColumnCount;

        for (int i = 0; i < InventorySettings.SlotCount; i++)
        {
            GameObject slot = Instantiate(_slotPrefab, transform);
        }
    }
}
