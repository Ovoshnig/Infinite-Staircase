using VContainer;
using VContainer.Unity;

public class AudioTuningMenuInstaller : IInstaller
{
    public void Install(IContainerBuilder builder)
    {
        builder.Register<SliderAudioMixerTunerMediatorFactory>(Lifetime.Singleton);

        builder.RegisterEntryPoint<AudioTuningMenuInitializer>(Lifetime.Singleton);
    }
}
