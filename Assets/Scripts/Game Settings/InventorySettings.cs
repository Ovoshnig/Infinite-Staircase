using System;
using UnityEngine;

[Serializable]
public class InventorySettings
{
    [field: SerializeField, Min(1)] public uint RowCount { get; private set; } = 4;
    [field: SerializeField, Min(1)] public uint ColumnCount { get; private set; } = 6;

    public uint SlotCount => RowCount * ColumnCount;
}
