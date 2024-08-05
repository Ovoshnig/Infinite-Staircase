using System;
using Zenject;

public class LookTuner : IInitializable, IDisposable
{
    private const string SensitivityKey = "Sensitivity";

    private readonly SettingsSaver _settingsSaver;
    private readonly GameSettingsInstaller.ControlSettings _controlSettings;
    private float _sensitivity;

    [Inject]
    public LookTuner(SettingsSaver settingsSaver, GameSettingsInstaller.ControlSettings controlSettings)
    {
        _settingsSaver = settingsSaver;
        _controlSettings = controlSettings;
    }

    public float Sensitivity
    {
        get
        {
            return _sensitivity;
        }
        set
        {
            if (value >= 0 && value <= _controlSettings.MaxSensitivity)
                _sensitivity = value;
        }
    }

    public void Initialize() => _sensitivity = _settingsSaver.LoadData(SensitivityKey, _controlSettings.DefaultSensitivity);

    public void Dispose() => _settingsSaver.SaveData(SensitivityKey, _sensitivity);
}
