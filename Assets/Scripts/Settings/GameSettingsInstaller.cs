using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Settings", menuName = "Scriptable Objects/Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    [SerializeField] private TimeSettings _timeSettings;
    [SerializeField] private ControlSettings _controlSettings;
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private AudioSettings _audioSettings;
    [SerializeField] private WorldGenerationSettings _worldGeneration;
    [SerializeField] private InventorySettings _inventorySettings;

    public InventorySettings InventorySettings1 { get => _inventorySettings; }

    public override void InstallBindings()
    {
        Container.BindInstance(_timeSettings);
        Container.BindInstance(_controlSettings);
        Container.BindInstance(_levelSettings);
        Container.BindInstance(_audioSettings);
        Container.BindInstance(_worldGeneration);
        Container.BindInstance(_inventorySettings);
    }

    [Serializable]
    public class TimeSettings
    {
        [field: SerializeField, Min(0f)] public float NormalTimeScale { get; private set; } = 1f;
        [field: SerializeField, Min(0f)] public float PauseTimeScale { get; private set; } = 0f;
    }

    [Serializable]
    public class ControlSettings
    {
        [field: SerializeField, Min(0f)] public float MinSensitivity { get; private set; }
        [field: SerializeField, Min(0f)] public float MaxSensitivity { get; private set; }

        public float DefaultSensitivity => (MinSensitivity + MaxSensitivity) / 2f;
    }

    [Serializable]
    public class LevelSettings
    {
        [field: SerializeField, Min(0)] public uint FirstGameplayLevel { get; private set; }
        [field: SerializeField, Min(1)] public uint GameplayLevelsCount { get; private set; }
        [field: SerializeField, Min(0.1f)] public float LevelTransitionDuration { get; private set; }

        public uint LastGameplayLevel => FirstGameplayLevel + GameplayLevelsCount - 1;
        public uint CreditsScene => LastGameplayLevel + 1;
    }

    [Serializable]
    public class AudioSettings
    {
        [field: SerializeField, Range(-80f, 20f)] public float MinVolume { get; private set; }
        [field: SerializeField, Range(-80f, 20f)] public float MaxVolume { get; private set; }
        [field: SerializeField, Min(0f)] public float SnapshotTransitionDuration { get; private set; }

        public float DefaultVolume => (MinVolume + MaxVolume) / 2f;
    }

    [Serializable]
    public class WorldGenerationSettings
    {
        [field: SerializeField] public int MinSeed { get; private set; }
        [field: SerializeField] public int MaxSeed { get; private set; }
    }

    [Serializable]
    public class InventorySettings
    {
        [field: SerializeField, Min(1)] public uint RowCount { get; private set; }
        [field: SerializeField, Min(1)] public uint ColumnCount { get; private set; }

        public uint SlotCount => RowCount * ColumnCount;
    }
}
