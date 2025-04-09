using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class InventoryWindowLifetimeScope : WindowLifetimeScope
{
    [SerializeField] private ItemDataRepository _itemDataRepository;
    [SerializeField] private ItemGenerator _itemGenerator;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<InventorySwitch>(Lifetime.Singleton).As<WindowSwitch>();

        builder.Register<InventorySaver>(Lifetime.Singleton);
        builder.Register<DraggedItemHolder>(Lifetime.Singleton);

        builder.RegisterInstance(_itemDataRepository);

        builder.Register(resolver =>
        {
            WindowView windowView = resolver.Resolve<WindowView>();
            return windowView.GetComponentInChildren<InventoryView>();
        }, Lifetime.Singleton);

        base.Configure(builder);
    }

    protected override void Start()
    {
        base.Start();

        Container.Inject(_itemGenerator);
    }
}
