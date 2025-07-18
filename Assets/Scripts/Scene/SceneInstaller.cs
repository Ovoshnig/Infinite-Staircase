using VContainer;
using VContainer.Unity;

public class SceneInstaller : IInstaller
{
    public void Install(IContainerBuilder builder) =>
        builder.RegisterEntryPoint<SceneSwitch>(Lifetime.Singleton).AsSelf();
}
