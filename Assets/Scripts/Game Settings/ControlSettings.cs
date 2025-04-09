using System;
using UnityEngine;

[Serializable]
public class ControlSettings
{
    [field: SerializeField, Min(0f)] public float MinSensitivity { get; private set; } = 0f;
    [field: SerializeField, Min(0f)] public float MaxSensitivity { get; private set; } = 50f;

    public float DefaultSensitivity => (MinSensitivity + MaxSensitivity) / 2f;
}
