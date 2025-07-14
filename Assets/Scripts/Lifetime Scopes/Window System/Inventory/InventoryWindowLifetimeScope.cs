using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class InventoryWindowLifetimeScope : WindowLifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<InventoryWindow>(Lifetime.Singleton).AsSelf().As<Window>();
        builder.RegisterEntryPoint<InventoryMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<InventoryWindowInventoryMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<RandomItemGenerator>(Lifetime.Singleton);
        
        builder.Register(resolver =>
        {
            Canvas windowCanvas = resolver.Resolve<Canvas>();
            return windowCanvas.GetComponentInChildren<InventoryView>();
        }, Lifetime.Singleton);

        base.Configure(builder);
    }
}
