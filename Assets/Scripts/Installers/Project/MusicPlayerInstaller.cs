using UnityEngine;
using Zenject;

public class MusicPlayerInstaller : MonoInstaller
{
    [SerializeField] private GameObject _musicPlayer;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ResourcesMusicLoader>().FromNew().AsSingle();
        Container.Bind<MusicQueue>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<SceneMusicMapper>().FromNew().AsSingle();

        Container.Bind<MusicPlayer>()
            .FromComponentInNewPrefab(_musicPlayer)
            .AsSingle()
            .NonLazy();
    }
}
