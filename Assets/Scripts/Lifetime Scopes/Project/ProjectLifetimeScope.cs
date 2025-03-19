using UnityEngine;
using UnityEngine.Audio;
using VContainer;
using VContainer.Unity;

public class ProjectLifetimeScope : LifetimeScope
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private MusicPlayer _musicPlayer;

    protected override void Configure(IContainerBuilder builder)
    {
#if !UNITY_EDITOR
        builder.RegisterEntryPoint<SplashScreenPasser>(Lifetime.Singleton).AsSelf();
#endif
        builder.RegisterEntryPoint<SceneSwitch>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<GamePauser>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<ScreenInputHandler>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<ScreenTuner>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<QualityTuner>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<LookTuner>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<AudioTuner>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<AudioMixerTuner>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<KeyBindingOverridesSaver>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<SaveStorage>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<SettingsStorage>(Lifetime.Singleton).AsSelf();

        builder.RegisterInstance(_gameSettings.TimeSettings);
        builder.RegisterInstance(_gameSettings.ControlSettings);
        builder.RegisterInstance(_gameSettings.LevelSettings);
        builder.RegisterInstance(_gameSettings.AudioSettings);
        builder.RegisterInstance(_gameSettings.WorldGeneration);
        builder.RegisterInstance(_gameSettings.PlayerSettings);
        builder.RegisterInstance(_gameSettings.InventorySettings);

        builder.RegisterInstance(_audioMixerGroup);

        builder.Register<IMusicLoader, ResourcesMusicLoader>(Lifetime.Singleton);
        builder.Register<ISceneMusicMapper, SceneMusicMapper>(Lifetime.Singleton);
        builder.Register<MusicQueue>(Lifetime.Singleton);
        builder.RegisterComponentInNewPrefab(_musicPlayer, Lifetime.Singleton)
            .DontDestroyOnLoad();
    }

    private void Start() => Container.Resolve<MusicPlayer>();
}
