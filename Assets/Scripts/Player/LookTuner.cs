using R3;
using System;
using Zenject;

public class LookTuner : IInitializable, IDisposable
{
    private readonly ReactiveProperty<float> _sensitivity = new();
    private readonly SettingsSaver _settingsSaver;
    private readonly GameSettingsInstaller.ControlSettings _controlSettings;

    [Inject]
    public LookTuner(SettingsSaver settingsSaver, GameSettingsInstaller.ControlSettings controlSettings)
    {
        _settingsSaver = settingsSaver;
        _controlSettings = controlSettings;
    }

    public ReactiveProperty<float> Sensitivity
    {
        get => _sensitivity;
        set => _sensitivity.Value = Math.Clamp(value.Value, 0f, _controlSettings.MaxSensitivity);
    }

    public void Initialize() => Sensitivity.Value = _settingsSaver.LoadData(SettingsConstants.SensitivityKey,
            _controlSettings.DefaultSensitivity);

    public void Dispose() => _settingsSaver.SaveData(SettingsConstants.SensitivityKey, Sensitivity.Value);
}