public class SensitivitySliderModel : SliderModel
{
    private readonly PlayerSettings _playerSettings;

    public SensitivitySliderModel(SettingsStorage settingsStorage, 
        PlayerSettings playerSettings) : base(settingsStorage) => 
        _playerSettings = playerSettings;

    public override float MinValue => _playerSettings.MinSensitivity;
    public override float MaxValue => _playerSettings.MaxSensitivity;

    protected override string DataKey => SettingsConstants.SensitivityKey;
    protected override float DefaultValue => _playerSettings.DefaultSensitivity;
}
