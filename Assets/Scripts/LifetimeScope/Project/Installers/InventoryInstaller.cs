using VContainer;
using VContainer.Unity;

public class InventoryInstaller : IInstaller
{
    public void Install(IContainerBuilder builder)
    {
        builder.Register<ItemDefinitionLoader>(Lifetime.Singleton);
        builder.Register<Inventory>(Lifetime.Singleton);
        builder.RegisterEntryPoint<InventorySaver>(Lifetime.Singleton);
    }
}
