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
        Container.BindInterfacesAndSelfTo<SaveSaver>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<SettingsSaver>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<SceneSwitch>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<GamePauser>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<AudioMixerGroup>().FromInstance(_audioMixerGroup).AsSingle();

        BindSettings(); 
    }

    private void BindSettings()
    {
        Container.BindInterfacesAndSelfTo<LookTuner>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<AudioTuner>().FromNew().AsSingle();
    }
}
