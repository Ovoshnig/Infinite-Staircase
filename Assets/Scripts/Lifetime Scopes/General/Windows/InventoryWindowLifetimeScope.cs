using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class InventoryWindowLifetimeScope : WindowLifetimeScope
{
    [SerializeField] private InventorySettings _inventorySettings;
    [SerializeField] private ItemGenerator _itemGenerator;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_inventorySettings);

        builder.RegisterEntryPoint<InventoryWindow>(Lifetime.Singleton).As<Window>();
        builder.RegisterEntryPoint<InventoryMediator>();

        builder.Register<Inventory>(Lifetime.Singleton);
        builder.Register<InventorySaver>(Lifetime.Singleton);
        builder.Register<ItemDefinitionLoader>(Lifetime.Singleton);
        builder.Register(resolver =>
        {
            Canvas windowCanvas = resolver.Resolve<Canvas>();
            return windowCanvas.GetComponentInChildren<InventoryView>();
        }, Lifetime.Singleton);

        base.Configure(builder);
    }

    protected override void Start()
    {
        base.Start();

        Container.Inject(_itemGenerator);
    }
}
