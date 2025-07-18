using VContainer;
using VContainer.Unity;

public class AudioTuningMenuInitializer : IStartable
{
    private readonly IObjectResolver _container;

    public AudioTuningMenuInitializer(IObjectResolver container) => _container = container;

    public void Start()
    {
        SliderAudioMixerTunerMediatorFactory sliderAudioMixerTunerMediatorFactory = _container
            .Resolve<SliderAudioMixerTunerMediatorFactory>();

        SoundSliderModel soundSliderModel = _container.Resolve<SoundSliderModel>();
        sliderAudioMixerTunerMediatorFactory.Create(soundSliderModel);

        MusicSliderModel musicSliderModel = _container.Resolve<MusicSliderModel>();
        sliderAudioMixerTunerMediatorFactory.Create(musicSliderModel);
    }
}
