using R3;

public class SoundSliderAudioMixerTunerMediator : Mediator
{
    private readonly SoundSliderModel _sliderModel;
    private readonly AudioMixerTuner _audioMixerTuner;

    public SoundSliderAudioMixerTunerMediator(SoundSliderModel sliderModel, AudioMixerTuner audioMixerTuner)
    {
        _sliderModel = sliderModel;
        _audioMixerTuner = audioMixerTuner;
    }

    public override void Initialize()
    {
        _sliderModel.Value
            .Subscribe(value => _audioMixerTuner.SetSoundVolume(value))
            .AddTo(CompositeDisposable);
    }
}
