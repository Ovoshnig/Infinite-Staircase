using System;
using UnityEngine;

[Serializable]
public class StaircaseGenerationSettings
{
    [field: SerializeField] public int SegmentCount { get; private set; } = 10;
}
