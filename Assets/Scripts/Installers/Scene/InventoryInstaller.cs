using UnityEngine;
using Zenject;

public class InventoryInstaller : MonoInstaller
{
    [SerializeField] private GameObject _inventoryCanvas;
    [SerializeField] private ItemDataRepository _itemDataRepository;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SlotKeeper>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<DraggedItemHolder>().FromNew().AsSingle();

        Container.Bind<ItemDataRepository>().FromInstance(_itemDataRepository).AsSingle();
    }

    [Inject]
    public void PostInstallBindings()
    {
        var slotViews = FindObjectsByType<SlotView>(FindObjectsSortMode.None);

        foreach (var slotView in slotViews)
            Container.Inject(slotView);
    }

    public override void Start() => Container.InstantiatePrefab(_inventoryCanvas);
}
