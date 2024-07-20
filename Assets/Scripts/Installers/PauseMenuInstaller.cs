using UnityEngine;
using Zenject;

public class PauseMenuInstaller : MonoInstaller
{
    [SerializeField] private GameObject _pauseMenuCanvas;

    public override void InstallBindings()
    {
        Container.BindFactory<PauseMenu, PauseMenu.Factory>()
            .FromComponentInNewPrefab(_pauseMenuCanvas)
            .AsSingle();
    }

    public override void Start()
    {
        var pauseMenuFactory = Container.Resolve<PauseMenu.Factory>();
        pauseMenuFactory.Create();
    }
}
