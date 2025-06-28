using UnityEngine;

[CreateAssetMenu(fileName = InventorySettingsConstants.FileName, 
    menuName = InventorySettingsConstants.MenuName)]
public class InventorySettings : ScriptableObject
{
    [field: SerializeField, Min(1)] public uint RowCount { get; private set; } = 4;
    [field: SerializeField, Min(1)] public uint ColumnCount { get; private set; } = 6;
    [field: SerializeField, Min(0.1f)] public float SpacingRatio { get; private set; } = 0.5f;
    [field: SerializeField, Range(0f, 0.45f)] public float ItemPaddingRatio { get; private set; } = 0.2f;

    public uint SlotCount => RowCount * ColumnCount;
}
