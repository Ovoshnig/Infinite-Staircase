using R3;
using VContainer;

public sealed class MusicSliderTuner : SliderTuner
{
    private AudioTuner _audioTuner;
    private AudioSettings _audioSettings;

    [Inject]
    public void Construct(AudioTuner audioTuner, AudioSettings audioSettings)
    {
        _audioTuner = audioTuner;
        _audioSettings = audioSettings;
    }

    protected override void Start()
    {
        Slider.minValue = _audioSettings.MinVolume;
        Slider.maxValue = _audioSettings.MaxVolume;

        _audioTuner.MusicVolume
            .Subscribe(Slider.SetValueWithoutNotify)
            .AddTo(this);

        base.Start();
    }

    protected override void OnSliderValueChanged(float value) => 
        _audioTuner.SetMusicVolume(value);
}
