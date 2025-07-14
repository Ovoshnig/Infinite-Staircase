using System;
using UnityEngine;

[Serializable]
public class TimeSettings
{
    [field: SerializeField, Min(0f)] public float NormalTimeScale { get; private set; } = 1f;
    [field: SerializeField, Min(0f)] public float PauseTimeScale { get; private set; } = 0f;
}
