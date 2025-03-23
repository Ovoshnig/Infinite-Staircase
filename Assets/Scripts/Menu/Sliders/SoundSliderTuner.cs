using VContainer;

public sealed class SoundSliderTuner : SliderTuner
{
    private AudioTuner _audioTuner;
    private AudioSettings _audioSettings;

    [Inject]
    public void Construct(AudioTuner audioTuner,
        AudioSettings audioSettings)
    {
        _audioTuner = audioTuner;
        _audioSettings = audioSettings;
    }

    protected override void Start()
    {
        base.Start();

        Slider.minValue = _audioSettings.MinVolume;
        Slider.maxValue = _audioSettings.MaxVolume;
        Slider.SetValueWithoutNotify(_audioTuner.SoundVolume.CurrentValue);
    }

    protected override void OnSliderValueChanged(float value) =>
        _audioTuner.SetSoundVolume(value);
}
