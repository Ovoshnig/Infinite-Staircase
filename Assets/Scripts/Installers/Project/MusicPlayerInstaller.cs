using UnityEngine;
using Zenject;

public class MusicPlayerInstaller : MonoInstaller
{
    [SerializeField] private GameObject _musicPlayer;

    public override void InstallBindings()
    {
        Container.Bind<MusicPlayer>()
            .FromComponentInNewPrefab(_musicPlayer)
            .AsSingle()
            .NonLazy();
    }
}
