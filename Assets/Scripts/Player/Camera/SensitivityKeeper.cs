using System;

public class SensitivityKeeper : DataKeeper<float>
{
    private readonly ControlSettings _controlSettings;

    public SensitivityKeeper(SettingsStorage settingsStorage, 
        ControlSettings controlSettings) : base(settingsStorage) => 
        _controlSettings = controlSettings;

    protected override string DataKey => SettingsConstants.SensitivityKey;
    protected override float DefaultValue => _controlSettings.DefaultSensitivity;

    public override void SetValue(float value)
    {
        value = Math.Clamp(value, _controlSettings.MinSensitivity, _controlSettings.MaxSensitivity);

        base.SetValue(value);
    }
}
