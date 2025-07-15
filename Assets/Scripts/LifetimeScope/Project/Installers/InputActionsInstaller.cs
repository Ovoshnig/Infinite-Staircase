using VContainer;
using VContainer.Unity;

public class InputActionsInstaller : IInstaller
{
    public void Install(IContainerBuilder builder) => builder.Register<InputActions>(Lifetime.Singleton);
}
