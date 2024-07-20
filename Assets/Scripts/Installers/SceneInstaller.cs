using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GameObject _inventoryCanvas;

    public override void InstallBindings()
    {
        GameObject inventoryCanvas = Instantiate(_inventoryCanvas);
    }
}
