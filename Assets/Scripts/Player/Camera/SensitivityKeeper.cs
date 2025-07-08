using System;

public class SensitivityKeeper : DataKeeper<float>
{
    private readonly PlayerSettings _playerSettings;

    public SensitivityKeeper(SettingsStorage settingsStorage, 
        PlayerSettings playerSettings) : base(settingsStorage) => 
        _playerSettings = playerSettings;

    protected override string DataKey => SettingsConstants.SensitivityKey;
    protected override float DefaultValue => _playerSettings.DefaultSensitivity;

    public override void SetValue(float value)
    {
        value = Math.Clamp(value, _playerSettings.MinSensitivity, _playerSettings.MaxSensitivity);

        base.SetValue(value);
    }
}
