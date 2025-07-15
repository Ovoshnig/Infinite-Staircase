public class SliderAudioMixerTunerMediatorFactory : MediatorFactory<SliderAudioMixerTunerMediator, SliderModel>
{
    private readonly AudioMixerTuner _audioMixerTuner;

    public SliderAudioMixerTunerMediatorFactory(AudioMixerTuner audioMixerTuner) => 
        _audioMixerTuner = audioMixerTuner;

    public override SliderAudioMixerTunerMediator Create(SliderModel sliderModel)
    {
        SliderAudioMixerTunerMediator mediator = new(sliderModel, _audioMixerTuner);
        mediator.Initialize();
        Disposables.Add(mediator);
        return mediator;
    }
}
