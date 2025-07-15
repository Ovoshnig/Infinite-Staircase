using VContainer;
using VContainer.Unity;

public class MusicInstaller : IInstaller
{
    private readonly MusicPlayerView _musicPlayerView;

    public MusicInstaller(MusicPlayerView musicPlayerView) =>
        _musicPlayerView = musicPlayerView;

    public void Install(IContainerBuilder builder)
    {
        builder.Register<IClipLoader, AddressablesClipLoader>(Lifetime.Singleton);
        builder.Register<ISceneMusicMapper, SceneMusicMapper>(Lifetime.Singleton);
        builder.Register<MusicQueue>(Lifetime.Singleton);
        builder.Register<MusicPlayer>(Lifetime.Singleton);

        builder.RegisterComponentInNewPrefab(_musicPlayerView, Lifetime.Singleton)
            .DontDestroyOnLoad();

        builder.RegisterEntryPoint<MusicPlayerMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<SceneSwitchMusicPlayerMediator>(Lifetime.Singleton);
    }
}
