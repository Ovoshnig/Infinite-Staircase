using R3;
using VContainer.Unity;

public class MusicSliderAudioMixerTunerMediator : Mediator, IStartable
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
    }

    public void Start()
    {
        _sliderModel.Value
            .Subscribe(value => _audioMixerTuner.SetMusicVolume(value))
            .AddTo(CompositeDisposable);
    }
}
