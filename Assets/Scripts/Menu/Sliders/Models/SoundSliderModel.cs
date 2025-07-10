public class SoundSliderModel : SliderModel
{
    private readonly AudioSettings _audioSettings;

    public SoundSliderModel(SettingsStorage settingsStorage,
        AudioSettings audioSettings) : base(settingsStorage) =>
        _audioSettings = audioSettings;

    public override float MinValue => _audioSettings.MinVolume;
    public override float MaxValue => _audioSettings.MaxVolume;

    protected override string DataKey => SettingsConstants.SoundVolumeKey;
    protected override float DefaultValue => _audioSettings.DefaultVolume;
}
