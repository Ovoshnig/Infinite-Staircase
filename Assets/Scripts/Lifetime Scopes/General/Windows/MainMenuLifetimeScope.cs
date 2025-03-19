using UnityEngine;
using VContainer.Unity;

public class MainMenuLifetimeScope : LifetimeScope
{
    [SerializeField] private GameObject _mainMenuCanvas;

    private void Start() => 
        Container.InjectGameObject(_mainMenuCanvas);
}
