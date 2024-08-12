using UnityEngine;
using VContainer;
using VContainer.Unity;

public class InventoryLifetimeScope : LifetimeScope
{
    [SerializeField] private InventoryView _inventoryCanvas;
    [SerializeField] private ItemDataRepository _itemDataRepository;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(_inventoryCanvas.GetComponent<InventoryView>(), Lifetime.Singleton);

        builder.Register<InventorySaver>(Lifetime.Singleton).AsSelf();
        builder.Register<DraggedItemHolder>(Lifetime.Singleton).AsSelf();

        builder.RegisterInstance(_itemDataRepository).AsSelf();
    }
}
