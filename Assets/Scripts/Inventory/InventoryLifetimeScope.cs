using UnityEngine;
using VContainer;
using VContainer.Unity;

public class InventoryLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register(resolver =>
        {
            Canvas windowCanvas = resolver.Resolve<Canvas>();
            return windowCanvas.GetComponentInChildren<InventoryView>(true);
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint<InventoryMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<RandomItemGenerator>(Lifetime.Singleton);
    }
}
