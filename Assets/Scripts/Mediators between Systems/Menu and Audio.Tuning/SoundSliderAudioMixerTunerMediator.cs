using R3;
using VContainer.Unity;

public class SoundSliderAudioMixerTunerMediator : Mediator, IStartable
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
    }

    public void Start()
    {
        _sliderModel.Value
            .Subscribe(value => _audioMixerTuner.SetSoundVolume(value))
            .AddTo(CompositeDisposable);
    }
}
