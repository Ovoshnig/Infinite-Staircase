using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Settings", menuName = "Scriptable Objects/Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    [SerializeField] private ControlSettings _controlSettings;
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private AudioSettings _audioSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(_controlSettings);
        Container.BindInstance(_levelSettings);
        Container.BindInstance(_audioSettings);
    }

    [Serializable]
    public class ControlSettings
    {
        public float MaxSensitivity;

        public float DefaultSensitivity => MaxSensitivity / 2f;
    }

    [Serializable]
    public class LevelSettings
    {
        public uint FirstGameplayLevel;
        public uint GameplayLevelsCount;
        public float LevelTransitionDuration;

        public uint LastGameplayLevel => FirstGameplayLevel + GameplayLevelsCount - 1;
        public uint CreditsScene => LastGameplayLevel + 1;
    }

    [Serializable]
    public class AudioSettings
    {
        public float MinVolume;
        public float MaxVolume;
        public float MusicFadeInDuration;

        public float DefaultVolume => (MinVolume + MaxVolume) / 2f;
    }
}
