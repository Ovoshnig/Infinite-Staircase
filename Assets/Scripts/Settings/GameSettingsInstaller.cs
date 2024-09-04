using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Settings", menuName = "Scriptable Objects/Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    [SerializeField] private ControlSettings _controlSettings;
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private AudioSettings _audioSettings;
    [SerializeField] private InventorySettings _inventorySettings;

    public InventorySettings InventorySettings1 { get => _inventorySettings; }

    public override void InstallBindings()
    {
        Container.BindInstance(_controlSettings);
        Container.BindInstance(_levelSettings);
        Container.BindInstance(_audioSettings);
        Container.BindInstance(_inventorySettings);
    }

    [Serializable]
    public class ControlSettings
    {
        [field: SerializeField] public float MinSensitivity { get; private set; }
        [field: SerializeField] public float MaxSensitivity { get; private set; }

        public float DefaultSensitivity => (MinSensitivity + MaxSensitivity) / 2f;
    }

    [Serializable]
    public class LevelSettings
    {
        [field: SerializeField] public uint FirstGameplayLevel { get; private set; }
        [field: SerializeField] public uint GameplayLevelsCount { get; private set; }
        [field: SerializeField] public float LevelTransitionDuration { get; private set; }

        public uint LastGameplayLevel => FirstGameplayLevel + GameplayLevelsCount - 1;
        public uint CreditsScene => LastGameplayLevel + 1;
    }

    [Serializable]
    public class AudioSettings
    {
        [field: SerializeField] public float MinVolume { get; private set; }
        [field: SerializeField] public float MaxVolume { get; private set; }
        [field: SerializeField] public float SnapshotTransitionDuration { get; private set; }

        public float DefaultVolume => (MinVolume + MaxVolume) / 2f;
    }

    [Serializable]
    public class InventorySettings
    {
        [field: SerializeField] public uint RowCount { get; private set; }
        [field: SerializeField] public uint ColumnCount { get; private set; }

        public uint SlotCount => RowCount * ColumnCount;
    }
}
