using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InventoryView))]
[RequireComponent(typeof(RectTransform))]
public class InventoryGridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private InventorySettings _inventorySettings;

    public void GenerateGrid()
    {
        if (!TryDestroyOld())
            return;

        GenerateNew();
    }

    private bool TryDestroyOld()
    {
        List<GameObject> directChildren = new();

        foreach (Transform child in transform)
            directChildren.Add(child.gameObject);

        if (directChildren.Count > 0)
        {
            foreach (var child in directChildren)
            {
                if (child.GetComponent<SlotView>() == null)
                {
                    Debug.LogError($"Invalid child object {child.name}, cancelling generation.", child);
                    return false;
                }
            }

            foreach (var child in directChildren)
                DestroyImmediate(child, false);
        }

        return true;
    }

    private void GenerateNew()
    {
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        int columns = (int)_inventorySettings.ColumnCount;
        int rows = (int)_inventorySettings.RowCount;

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;

        RectTransform rectTransform = transform as RectTransform;
        float containerWidth = rectTransform.rect.width;
        float containerHeight = rectTransform.rect.height;
        float spacingRatio = _inventorySettings.SpacingRatio;

        float cellWidth = containerWidth / (columns + (columns + 1) * spacingRatio);
        float cellHeight = containerHeight / (rows + (rows + 1) * spacingRatio);
        float cellSize = Mathf.Min(cellWidth, cellHeight);
        float spacing = cellSize * spacingRatio;
        int padding = Mathf.RoundToInt(spacing);

        grid.cellSize = new Vector2(cellSize, cellSize);
        grid.spacing = new Vector2(spacing, spacing);
        grid.padding = new RectOffset(padding, padding, padding, padding);

        float itemPadding = _inventorySettings.ItemPaddingRatio * cellSize;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                var slot = Instantiate(_slotPrefab, transform);
                slot.name = $"Slot ({i}, {j})";
                slot.GetComponent<SlotView>().SetItemPadding(itemPadding);
            }
        }

        Debug.Log("Generation successfully completed");
    }
}
