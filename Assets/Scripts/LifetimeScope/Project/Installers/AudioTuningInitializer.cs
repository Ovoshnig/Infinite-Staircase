using VContainer;
using VContainer.Unity;

public class AudioTuningInitializer : IStartable
{
    private readonly IObjectResolver _resolver;

    public AudioTuningInitializer(IObjectResolver resolver) => _resolver = resolver;

    public void Start()
    {
        SliderAudioMixerTunerMediatorFactory sliderAudioMixerTunerMediatorFactory = _resolver
            .Resolve<SliderAudioMixerTunerMediatorFactory>();

        SoundSliderModel soundSliderModel = _resolver.Resolve<SoundSliderModel>();
        sliderAudioMixerTunerMediatorFactory.Create(soundSliderModel);

        MusicSliderModel musicSliderModel = _resolver.Resolve<MusicSliderModel>();
        sliderAudioMixerTunerMediatorFactory.Create(musicSliderModel);
    }
}
