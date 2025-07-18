using VContainer;
using VContainer.Unity;

public class KeyBindingInstaller : IInstaller
{
    public void Install(IContainerBuilder builder) =>
        builder.RegisterEntryPoint<KeyBindingOverridesSaver>(Lifetime.Singleton).AsSelf();
}
