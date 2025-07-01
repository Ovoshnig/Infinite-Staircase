using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.UI;

public class InventoryGridGenerator : MonoBehaviour
{
    [SerializeField] private RectTransform _parentTransform;
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private InventorySettings _inventorySettings;

    public bool TryGenerate()
    {
        if (!Application.isEditor)
            return false;

        if (Application.isPlaying)
        {
            Debug.LogWarning("Slot generation is disabled during Play Mode.");
            return false;
        }

        int undoGroup = Undo.GetCurrentGroup();
        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Generate Inventory Grid");

        Undo.RegisterFullObjectHierarchyUndo(_parentTransform.gameObject, "Generate Inventory Grid");

        if (TryDestroyOld())
        {
            GenerateNew();

            EditorUtility.SetDirty(_parentTransform.gameObject);
            EditorSceneManager.MarkSceneDirty(_parentTransform.gameObject.scene);

            return true;
        }

        return false;
    }

    private bool TryDestroyOld()
    {
        List<GameObject> directChildren = _parentTransform
                    .Cast<Transform>()
                    .Select(t => t.gameObject)
                    .ToList();

        if (directChildren.Any())
        {
            foreach (var directChild in directChildren)
            {
                if (directChild.GetComponent<SlotView>() == null)
                {
                    Debug.LogError($"Invalid child object {directChild.name}, destroying and " +
                        "generation of inventory grid cancelled", directChild);

                    return false;
                }
            }

            foreach (var directChild in directChildren)
                DestroyImmediate(directChild, false);
        }

        return true;
    }

    private void GenerateNew()
    {
        GridLayoutGroup grid = _parentTransform.GetComponent<GridLayoutGroup>();
        int columns = (int)_inventorySettings.ColumnCount;
        int rows = (int)_inventorySettings.RowCount;

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;

        float containerWidth = _parentTransform.rect.width;
        float containerHeight = _parentTransform.rect.height;

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
                GameObject slot = Instantiate(_slotPrefab, _parentTransform);
                slot.name = $"Slot ({i}, {j})";

                slot.GetComponent<SlotView>().SetItemPadding(itemPadding);
            }
        }

        Debug.Log("Generation successfully completed");
    }
}
