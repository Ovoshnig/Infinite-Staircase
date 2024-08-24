using R3;
using System;
using Zenject;

public class LookTuner : IInitializable, IDisposable
{
    private readonly ReactiveProperty<float> _sensitivity = new(0f);
    private readonly SettingsSaver _settingsSaver;
    private readonly GameSettingsInstaller.ControlSettings _controlSettings;

    [Inject]
    public LookTuner(SettingsSaver settingsSaver, GameSettingsInstaller.ControlSettings controlSettings)
    {
        _settingsSaver = settingsSaver;
        _controlSettings = controlSettings;
    }

    public float Sensitivity
    {
        get => _sensitivity.Value;
        set => _sensitivity.Value = Math.Clamp(value, 0f, _controlSettings.MaxSensitivity);
    }

    public ReadOnlyReactiveProperty<float> SensitivityReactive => _sensitivity;

    public void Initialize() => Sensitivity = _settingsSaver.LoadData(SettingsConstants.SensitivityKey,
            _controlSettings.DefaultSensitivity);

    public void Dispose() => _settingsSaver.SaveData(SettingsConstants.SensitivityKey, Sensitivity);
}
