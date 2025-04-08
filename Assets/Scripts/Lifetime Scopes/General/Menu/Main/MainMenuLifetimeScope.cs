using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainMenuLifetimeScope : LifetimeScope
{
    [SerializeField] private Canvas _mainMenuCanvas;

    protected override void Configure(IContainerBuilder builder) => 
        builder.RegisterEntryPoint<MenuInputHandler>(Lifetime.Singleton).AsSelf();

    private void Start() => 
        Container.InjectGameObject(_mainMenuCanvas.gameObject);
}
