using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup), 
                  typeof(Image))]
public class InventoryGridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private GameSettings _settings;

    private GridLayoutGroup _gridLayoutGroup;
    private Image _panelImage;

    private InventorySettings InventorySettings => _settings.InventorySettings;

    private void OnValidate()
    {
        if (_gridLayoutGroup == null)
            _gridLayoutGroup = GetComponent<GridLayoutGroup>();

        if (_panelImage == null)
            _panelImage = GetComponent<Image>();
    }

    [ContextMenu(nameof(PlaceSlots))]
    private void PlaceSlots()
    {
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
