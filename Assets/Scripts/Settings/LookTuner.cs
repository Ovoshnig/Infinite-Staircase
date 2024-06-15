using System;
using Zenject;

public class LookTuner : IDisposable
{
    private const string SensitivityKey = "Sensitivity";

    private readonly DataSaver _dataSaver;
    private readonly GameSettingsInstaller.ControlSettings _controlSettings;
    private float _sensitivity;

    [Inject]
    public LookTuner(DataSaver dataSaver, GameSettingsInstaller.ControlSettings controlSettings)
    {
        _dataSaver = dataSaver;
        _controlSettings = controlSettings;
        _sensitivity = _dataSaver.LoadData(SensitivityKey, _controlSettings.DefaultSensitivity);
        Sensitivity = _sensitivity;
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

    public void Dispose() => _dataSaver.SaveData(SensitivityKey, _sensitivity);
}
