using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private AudioSource _musicSource;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SplashScreenPasser>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GamePauser>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DataSaver>().FromNew().AsSingle().NonLazy();
        Container.Bind<AudioMixerGroup>().FromInstance(_audioMixerGroup).AsSingle().NonLazy();
        BindSettings();
        BindMusicPlayer();
    }

    private void BindSettings()
    {
        Container.BindInterfacesAndSelfTo<SceneSwitch>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LookTuner>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AudioTuner>().FromNew().AsSingle().NonLazy();
    }

    private void BindMusicPlayer()
    {
        Container.Bind<AudioSource>().WithId("musicSource").FromInstance(_musicSource).AsTransient().NonLazy();
        Container.BindInterfacesAndSelfTo<MusicPlayer>().FromNew().AsSingle().NonLazy();
    }
}
