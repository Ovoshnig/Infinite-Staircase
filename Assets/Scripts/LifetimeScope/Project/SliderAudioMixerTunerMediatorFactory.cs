public class SliderAudioMixerTunerMediatorFactory : MediatorFactory<SliderAudioMixerTunerMediator, SliderModel>
{
    private readonly AudioMixerTuner _audioMixerTuner;

    public SliderAudioMixerTunerMediatorFactory(AudioMixerTuner audioMixerTuner) =>
        _audioMixerTuner = audioMixerTuner;

    protected override SliderAudioMixerTunerMediator CreateMediatorInstance(SliderModel sliderModel) =>
        new(sliderModel, _audioMixerTuner);
}
