using Zenject;

public sealed class SoundSliderTuner : SliderTuner
{
    private AudioTuner _audioTuner;
    private GameSettingsInstaller.AudioSettings _audioSettings;

    [Inject]
    private void Construct(AudioTuner audioTuner, 
        GameSettingsInstaller.AudioSettings audioSettings)
    {
        _audioTuner = audioTuner;
        _audioSettings = audioSettings;
    }

    protected override float MinValue => _audioSettings.MinVolume;

    protected override float MaxValue => _audioSettings.MaxVolume;

    protected override float InitialValue => _audioTuner.SoundVolume.CurrentValue;

    protected override void OnSliderValueChanged(float value) => 
        _audioTuner.SoundVolume.Value = value;
}
