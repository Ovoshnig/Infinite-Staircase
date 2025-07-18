using VContainer;
using VContainer.Unity;

public class GamePauseInstaller : IInstaller
{
    public void Install(IContainerBuilder builder) =>
        builder.RegisterEntryPoint<GamePauser>(Lifetime.Singleton).AsSelf();
}
