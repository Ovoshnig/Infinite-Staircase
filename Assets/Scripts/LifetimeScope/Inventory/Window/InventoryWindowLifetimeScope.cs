using VContainer;
using VContainer.Unity;

public sealed class InventoryWindowLifetimeScope : WindowLifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<InventoryWindow>(Lifetime.Singleton).AsSelf().As<Window>();
        builder.RegisterEntryPoint<InventoryWindowInventoryMediator>(Lifetime.Singleton);

        base.Configure(builder);
    }
}
