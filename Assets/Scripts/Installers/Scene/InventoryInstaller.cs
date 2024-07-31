using UnityEngine;
using Zenject;

public class InventoryInstaller : MonoInstaller
{
    [SerializeField] private GameObject _inventoryCanvas;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<DraggedItemHolder>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<SlotKeeper>().FromNew().AsSingle();
    }

    public override void Start() => Container.InstantiatePrefab(_inventoryCanvas);
}
