using VContainer;
using VContainer.Unity;

public class AudioMusicInstaller : IInstaller
{
    private readonly MusicPlayerView _musicPlayerView;

    public AudioMusicInstaller(MusicPlayerView musicPlayerView) =>
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
    }
}
