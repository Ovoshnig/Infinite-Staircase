using UnityEngine;
using UnityEngine.Audio;
using VContainer;
using VContainer.Unity;

public class GlobalLifetimeScope : LifetimeScope
{
    [SerializeField] private AudioMixerGroup _audioMixerGroup;

    protected override void Configure(IContainerBuilder builder)
    {
#if !UNITY_EDITOR
        builder.Register<SplashScreenPasser>(Lifetime.Singleton).AsSelf();
#endif
        builder.Register<SaveSaver>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<SettingsSaver>(Lifetime.Singleton).AsSelf();
        builder.Register<SceneSwitch>(Lifetime.Singleton).AsSelf();
        builder.Register<GamePauser>(Lifetime.Singleton).AsSelf();
        builder.Register<LookTuner>(Lifetime.Singleton).AsSelf();
        builder.Register<AudioTuner>(Lifetime.Singleton).AsSelf();

        builder.RegisterInstance(_audioMixerGroup).AsSelf();
    }
}
