using UnityEngine;
using Zenject;

public class PauseMenuInstaller : MonoInstaller
{
    [SerializeField] private GameObject _pauseMenuCanvas;

    public override void InstallBindings()
    {
        Container.Bind<PauseMenuSwitch>()
            .FromComponentInNewPrefab(_pauseMenuCanvas)
            .AsSingle()
            .NonLazy();
    }
}
