using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PauseMenuLifetimeScope : LifetimeScope
{
    [SerializeField] private PauseMenuSwitch _pauseMenuSwitch;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(_pauseMenuSwitch, Lifetime.Singleton);

        builder.RegisterEntryPoint<MenuInputHandler>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<PauseMenuSwitchGamePauserMediator>(Lifetime.Singleton);
    }

    private void Start()
    {
        GameObject pauseMenu = Container.Resolve<PauseMenuSwitch>().gameObject;
        Container.InjectGameObject(pauseMenu);
    }
}
