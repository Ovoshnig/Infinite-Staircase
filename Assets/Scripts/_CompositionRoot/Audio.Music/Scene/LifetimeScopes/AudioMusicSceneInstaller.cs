using VContainer;
using VContainer.Unity;

public class AudioMusicSceneInstaller : IInstaller
{
    public void Install(IContainerBuilder builder) =>
        builder.RegisterEntryPoint<MusicPlayerSceneSwitchMediator>(Lifetime.Singleton);
}
