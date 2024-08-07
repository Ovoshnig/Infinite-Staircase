using UnityEngine;
using Zenject;

public class InventoryInstaller : MonoInstaller
{
    [SerializeField] private GameObject _inventoryCanvas;
    [SerializeField] private ItemDataRepository _itemDataRepository;

    public override void InstallBindings()
    {
        Container.Bind<InventorySaver>()
            .FromComponentInNewPrefab(_inventoryCanvas)
            .AsSingle();

        Container.BindInterfacesAndSelfTo<SlotKeeper>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<DraggedItemHolder>().FromNew().AsSingle();

        Container.Bind<ItemDataRepository>().FromInstance(_itemDataRepository).AsSingle();
    }
}
