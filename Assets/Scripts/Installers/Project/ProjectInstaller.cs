using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private AudioMixerGroup _audioMixerGroup;

    public override void InstallBindings()
    {
#if !UNITY_EDITOR
        Container.BindInterfacesAndSelfTo<SplashScreenPasser>().FromNew().AsSingle();
#endif
        Container.BindInterfacesAndSelfTo<SaveStorage>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<SettingsStorage>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<SceneSwitch>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<GamePauser>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<AudioMixerGroup>().FromInstance(_audioMixerGroup).AsSingle();

        Container.BindInterfacesAndSelfTo<ScreenTuner>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<LookTuner>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<AudioTuner>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<AudioMixerTuner>().FromNew().AsSingle();
    }
}
