using UnityEngine;
using VContainer;
using VContainer.Unity;

public class InventoryLifetimeScope : LifetimeScope
{
    [SerializeField] private InventorySwitch _inventorySwitch;
    [SerializeField] private ItemDataRepository _itemDataRepository;
    [SerializeField] private ItemGenerator _itemGenerator;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<InventorySaver>(Lifetime.Singleton);
        builder.Register<DraggedItemHolder>(Lifetime.Singleton);

        builder.RegisterInstance(_itemDataRepository);

        builder.RegisterComponentInNewPrefab(_inventorySwitch, Lifetime.Singleton);

        builder.Register(resolver =>
        {
            InventorySwitch inventorySwitch = resolver.Resolve<InventorySwitch>();
            InventoryView inventoryView = inventorySwitch.GetComponentInChildren<InventoryView>();

            return inventoryView;
        }, Lifetime.Singleton);
    }

    private void Start()
    {
        GameObject inventory = Container.Resolve<InventorySwitch>().gameObject;
        Container.InjectGameObject(inventory);

        Container.Inject(_itemGenerator);
    }
}
