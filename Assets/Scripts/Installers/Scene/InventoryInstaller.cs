using UnityEngine;
using Zenject;

public class InventoryInstaller : MonoInstaller
{
    [SerializeField] private GameObject _inventoryCanvas;

    public override void InstallBindings()
    {
    }

    public override void Start() => Container.InstantiatePrefab(_inventoryCanvas);
}
