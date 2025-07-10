using R3;

public class MusicSliderAudioMixerTunerMediator : Mediator
{
    private readonly MusicSliderModel _sliderModel;
    private readonly AudioMixerTuner _audioMixerTuner;

    public MusicSliderAudioMixerTunerMediator(MusicSliderModel sliderModel, AudioMixerTuner audioMixerTuner)
    {
        _sliderModel = sliderModel;
        _audioMixerTuner = audioMixerTuner;
    }

    public override void Initialize()
    {
        _sliderModel.Value
            .Subscribe(value => _audioMixerTuner.SetMusicVolume(value))
            .AddTo(CompositeDisposable);
    }
}
