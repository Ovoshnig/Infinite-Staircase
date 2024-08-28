using UnityEngine;
using Zenject;

public class MusicPlayerInstaller : MonoInstaller
{
    [SerializeField] private GameObject _musicPlayer;

    public override void InstallBindings()
    {
        Container.Bind<IMusicLoader>().To<ResourcesMusicLoader>().FromNew().AsSingle();
        Container.Bind<MusicQueue>().FromNew().AsSingle();
        Container.Bind<ISceneMusicMapper>().To<SceneMusicMapper>().FromNew().AsSingle();

        Container.Bind<MusicPlayer>()
            .FromComponentInNewPrefab(_musicPlayer)
            .AsSingle()
            .NonLazy();
    }
}
