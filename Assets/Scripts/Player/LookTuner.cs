using R3;
using System;
using Zenject;

public class LookTuner : IInitializable, IDisposable
{
    private readonly ReactiveProperty<float> _sensitivity = new(0f);
    private readonly SettingsSaver _settingsSaver;
    private readonly GameSettingsInstaller.ControlSettings _controlSettings;
    private IDisposable _disposable;

    [Inject]
    public LookTuner(SettingsSaver settingsSaver, GameSettingsInstaller.ControlSettings controlSettings)
    {
        _settingsSaver = settingsSaver;
        _controlSettings = controlSettings;
    }

    public ReactiveProperty<float> Sensitivity => _sensitivity;

    public void Initialize()
    {
        Sensitivity.Value = _settingsSaver.LoadData(SettingsConstants.SensitivityKey,
            _controlSettings.DefaultSensitivity);

        _disposable = Sensitivity
            .Subscribe(value =>
            _sensitivity.Value = Math.Clamp(value, 0f, _controlSettings.MaxSensitivity));
    }

    public void Dispose()
    {
        _settingsSaver.SaveData(SettingsConstants.SensitivityKey, Sensitivity.Value);

        _disposable?.Dispose();
    }
}
