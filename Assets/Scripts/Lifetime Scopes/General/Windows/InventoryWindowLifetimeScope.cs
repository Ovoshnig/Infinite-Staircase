using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class InventoryWindowLifetimeScope : WindowLifetimeScope
{
    [SerializeField] private ItemGenerator _itemGenerator;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<InventoryWindow>(Lifetime.Singleton).AsSelf().As<Window>();
        builder.RegisterEntryPoint<InventoryMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<InventoryWindowInventoryMediator>(Lifetime.Singleton);
        
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
