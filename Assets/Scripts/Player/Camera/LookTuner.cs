using R3;
using System;
using VContainer.Unity;

public class LookTuner : IPostInitializable, IDisposable
{
    private readonly ReactiveProperty<float> _sensitivity = new();
    private readonly SettingsStorage _settingsStorage;
    private readonly ControlSettings _controlSettings;

    public LookTuner(SettingsStorage settingsStorage, ControlSettings controlSettings)
    {
        _settingsStorage = settingsStorage;
        _controlSettings = controlSettings;
    }

    public ReadOnlyReactiveProperty<float> Sensitivity => _sensitivity;

    public void PostInitialize()
    {
        float sensitivity = _settingsStorage.Get(SettingsConstants.SensitivityKey, _controlSettings.DefaultSensitivity);
        SetSensitivity(sensitivity);
    }

    public void Dispose() =>
        _settingsStorage.Set(SettingsConstants.SensitivityKey, _sensitivity.Value);

    public void SetSensitivity(float value)
    {
        value = Math.Clamp(value, _controlSettings.MinSensitivity, _controlSettings.MaxSensitivity);
        _sensitivity.Value = value;
    }
}
