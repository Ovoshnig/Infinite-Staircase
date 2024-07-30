using UnityEngine;
using Zenject;

public class PauseMenuInstaller : MonoInstaller
{
    [SerializeField] private GameObject _pauseMenuCanvas;

    public override void InstallBindings()
    {
        Container.Bind<PauseMenu>()
            .FromComponentInNewPrefab(_pauseMenuCanvas)
            .AsSingle();
    }

    public override void Start() => Container.InstantiatePrefab(_pauseMenuCanvas);
}
