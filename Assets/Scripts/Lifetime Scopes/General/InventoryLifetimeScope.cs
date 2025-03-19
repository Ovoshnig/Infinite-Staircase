using UnityEngine;
using VContainer;
using VContainer.Unity;

public class InventoryLifetimeScope : LifetimeScope
{
    [SerializeField] private InventorySwitch _inventorySwitch;
    [SerializeField] private ItemDataRepository _itemDataRepository;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<InventorySaver>(Lifetime.Singleton);
        builder.Register<DraggedItemHolder>(Lifetime.Singleton);

        builder.RegisterInstance(_itemDataRepository);
    }

    private void Start() => Container.Instantiate(_inventorySwitch);
}
