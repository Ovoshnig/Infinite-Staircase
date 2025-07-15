using UnityEngine;
using UnityEngine.Audio;
using VContainer;
using VContainer.Unity;

public class ProjectLifetimeScope : LifetimeScope
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private MusicPlayerView _musicPlayerView;

    protected override void Configure(IContainerBuilder builder)
    {
#if !UNITY_EDITOR
        builder.RegisterEntryPoint<SplashScreenPasser>(Lifetime.Singleton).AsSelf();
#endif
        builder.RegisterEntryPoint<SaveStorage>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<SettingsStorage>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<KeyBindingOverridesSaver>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<SensitivitySliderModel>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<SceneSwitch>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<GamePauser>(Lifetime.Singleton).AsSelf();
        
        builder.Register<InputActions>(Lifetime.Singleton);

        new GameSettingsInstaller(_gameSettings).Install(builder);
        new ScreenInstaller().Install(builder);
        new AudioTuningInstaller(_audioMixerGroup).Install(builder);
        new MusicInstaller(_musicPlayerView).Install(builder);
        new InventoryInstaller().Install(builder);

        builder.RegisterEntryPoint<AudioTuningInitializer>(Lifetime.Singleton);
    }
}
