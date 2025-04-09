using System;
using UnityEngine;

[Serializable]
public class WorldGenerationSettings
{
    [field: SerializeField] public int MinSeed { get; private set; } = -99999999;
    [field: SerializeField] public int MaxSeed { get; private set; } = 999999999;
}
