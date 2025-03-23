using UnityEngine;
using VContainer.Unity;

public class PauseMenuLifetimeScope : LifetimeScope
{
    [SerializeField] private GameObject _pauseMenuCanvas;

    private void Start() => Container.Instantiate(_pauseMenuCanvas);
}
