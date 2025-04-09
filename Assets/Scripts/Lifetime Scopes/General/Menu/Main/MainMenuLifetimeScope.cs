using UnityEngine;
using VContainer.Unity;

public class MainMenuLifetimeScope : LifetimeScope
{
    [SerializeField] private Canvas _mainMenuCanvas;

    private void Start() => 
        Container.InjectGameObject(_mainMenuCanvas.gameObject);
}
