using System;

public class MusicVolumeKeeper : DataKeeper<float>
{
    private readonly AudioSettings _audioSettings;

    public MusicVolumeKeeper(SettingsStorage settingsStorage,
        AudioSettings audioSettings) : base(settingsStorage) =>
        _audioSettings = audioSettings;
    
    protected override string DataKey => SettingsConstants.MusicVolumeKey;
    protected override float DefaultValue => _audioSettings.DefaultVolume;

    public override void SetValue(float value)
    {
        value = Math.Clamp(value, _audioSettings.MinVolume, _audioSettings.MaxVolume);

        base.SetValue(value);
    }
}
