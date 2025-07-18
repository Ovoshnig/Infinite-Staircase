using VContainer;
using VContainer.Unity;

public class AudioTuningGamePauseInstaller : IInstaller
{
    public void Install(IContainerBuilder builder) =>
        builder.RegisterEntryPoint<AudioMixerTunerGamePauserMediator>(Lifetime.Singleton);
}
