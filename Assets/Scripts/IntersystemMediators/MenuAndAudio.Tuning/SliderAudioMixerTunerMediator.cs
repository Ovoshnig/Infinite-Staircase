using R3;

public class SliderAudioMixerTunerMediator : Mediator
{
    private readonly SliderModel _sliderModel;
    private readonly AudioMixerTuner _audioMixerTuner;

    public SliderAudioMixerTunerMediator(SliderModel sliderModel, AudioMixerTuner audioMixerTuner)
    {
        _sliderModel = sliderModel;
        _audioMixerTuner = audioMixerTuner;
    }

    public override void Initialize()
    {
        _sliderModel.Value
            .Subscribe(value =>
            {
                if (_sliderModel is SoundSliderModel)
                    _audioMixerTuner.SetSoundVolume(value);
                else if (_sliderModel is MusicSliderModel)
                    _audioMixerTuner.SetMusicVolume(value);
            })
            .AddTo(CompositeDisposable);
    }
}
