using System;
using UnityEngine;

[Serializable]
public class AudioSettings
{
    [field: SerializeField, Range(-80f, 20f)] public float MinVolume { get; private set; } = -80f;
    [field: SerializeField, Range(-80f, 20f)] public float MaxVolume { get; private set; } = 0f;
    [field: SerializeField, Min(0f)] public float SnapshotTransitionDuration { get; private set; } = 0f;

    public float DefaultVolume => (MinVolume + MaxVolume) / 2f;
}
