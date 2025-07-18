using VContainer;
using VContainer.Unity;

public class MenuInstaller : IInstaller
{
    public void Install(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<SoundSliderModel>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<MusicSliderModel>(Lifetime.Singleton).AsSelf();
    }
}
