using System;
using UnityEngine;

[Serializable]
public class SceneSettings
{
    [field: SerializeField, Min(0)] public uint FirstGameplayLevel { get; private set; } = 1;
    [field: SerializeField, Min(1)] public uint GameplayLevelsCount { get; private set; } = 2;
    [field: SerializeField, Min(0.1f)] public float LevelTransitionDuration { get; private set; } = 5f;

    public uint LastGameplayLevel => FirstGameplayLevel + GameplayLevelsCount - 1;
    public uint CreditsScene => LastGameplayLevel + 1;
}
