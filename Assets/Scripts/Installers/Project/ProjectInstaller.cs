using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private AudioMixerGroup _audioMixerGroup;

    public override void InstallBindings()
    {
#if !UNITY_EDITOR
        Container.BindInterfacesAndSelfTo<SplashScreenPasser>().FromNew().AsSingle().NonLazy();
#endif
        Container.BindInterfacesAndSelfTo<GamePauser>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DataSaver>().FromNew().AsSingle().NonLazy();
        Container.Bind<AudioMixerGroup>().FromInstance(_audioMixerGroup).AsSingle().NonLazy();
        BindSettings();
    }

    private void BindSettings()
    {
        Container.BindInterfacesAndSelfTo<SceneSwitch>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LookTuner>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AudioTuner>().FromNew().AsSingle().NonLazy();
    }
}
