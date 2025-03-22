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

    protected override float MinValue => _audioSettings.MinVolume;
    protected override float MaxValue => _audioSettings.MaxVolume;

    protected override void Start()
    {
        base.Start();

        SetSliderValue(_audioTuner.MusicVolume.CurrentValue);
    }

    protected override void OnSliderValueChanged(float value) => 
        _audioTuner.SetMusicVolume(value);
}
