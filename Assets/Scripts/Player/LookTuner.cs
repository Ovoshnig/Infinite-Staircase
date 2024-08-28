using R3;
using System;
using Zenject;

public class LookTuner : IInitializable, IDisposable
{
    private readonly ReactiveProperty<float> _sensitivity = new(0f);
    private readonly SettingsStorage _settingsStorage;
    private readonly GameSettingsInstaller.ControlSettings _controlSettings;
    private IDisposable _disposable;

    [Inject]
    public LookTuner(SettingsStorage settingsStorage, GameSettingsInstaller.ControlSettings controlSettings)
    {
        _settingsStorage = settingsStorage;
        _controlSettings = controlSettings;
    }

    public ReactiveProperty<float> Sensitivity => _sensitivity;

    public void Initialize()
    {
        Sensitivity.Value = _settingsStorage.Get(SettingsConstants.SensitivityKey,
            _controlSettings.DefaultSensitivity);

        _disposable = Sensitivity
            .Subscribe(value =>
            _sensitivity.Value = Math.Clamp(value, 0f, _controlSettings.MaxSensitivity));
    }

    public void Dispose()
    {
        _settingsStorage.Set(SettingsConstants.SensitivityKey, Sensitivity.Value);

        _disposable?.Dispose();
    }
}
