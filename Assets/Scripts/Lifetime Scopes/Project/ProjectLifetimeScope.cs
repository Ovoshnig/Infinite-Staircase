using UnityEngine;
using UnityEngine.Audio;
using VContainer;
using VContainer.Unity;

public class ProjectLifetimeScope : LifetimeScope
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private MusicPlayer _musicPlayer;
    [SerializeField] private InventorySettings _inventorySettings;

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
        builder.RegisterEntryPoint<ScreenInputHandler>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<FullScreenTuner>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<VSyncTuner>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<ResolutionTuner>(Lifetime.Singleton).AsSelf();

        builder.Register<InputActions>(Lifetime.Singleton);

        builder.RegisterInstance(_gameSettings.TimeSettings);
        builder.RegisterInstance(_gameSettings.SceneSettings);
        builder.RegisterInstance(_gameSettings.AudioSettings);
        builder.RegisterInstance(_gameSettings.WorldGeneration);
        builder.RegisterInstance(_gameSettings.StaircaseGeneration);
        builder.RegisterInstance(_gameSettings.PlayerSettings);

        builder.RegisterInstance(_audioMixerGroup);
        builder.Register<AudioMixerTuner>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<GamePauserAudioMixerTunerMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<SoundSliderModel>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<MusicSliderModel>(Lifetime.Singleton).AsSelf();
        builder.Register<SliderAudioMixerTunerMediatorFactory>(Lifetime.Singleton);

        builder.Register<IClipLoader, AddressablesClipLoader>(Lifetime.Singleton);
        builder.Register<ISceneMusicMapper, SceneMusicMapper>(Lifetime.Singleton);
        builder.Register<MusicQueue>(Lifetime.Singleton);
        builder.RegisterComponentInNewPrefab(_musicPlayer, Lifetime.Singleton)
            .DontDestroyOnLoad();

        builder.RegisterInstance(_inventorySettings);
        builder.Register<ItemDefinitionLoader>(Lifetime.Singleton);
        builder.Register<Inventory>(Lifetime.Singleton);
        builder.RegisterEntryPoint<InventorySaver>(Lifetime.Singleton);
    }

    private void Start()
    {
        SliderAudioMixerTunerMediatorFactory sliderAudioMixerTunerMediatorFactory = Container
            .Resolve<SliderAudioMixerTunerMediatorFactory>();

        SoundSliderModel soundSliderModel = Container.Resolve<SoundSliderModel>();
        sliderAudioMixerTunerMediatorFactory.Create(soundSliderModel);

        MusicSliderModel musicSliderModel = Container.Resolve<MusicSliderModel>();
        sliderAudioMixerTunerMediatorFactory.Create(musicSliderModel);

        Container.Resolve<MusicPlayer>();
    }
}
