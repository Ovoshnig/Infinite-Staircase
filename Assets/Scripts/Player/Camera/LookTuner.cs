using R3;
using System;
using Zenject;

public class LookTuner : IInitializable, IDisposable
{
    private readonly ReactiveProperty<float> _sensitivity = new(0f);
    private readonly SettingsStorage _settingsStorage;
    private readonly GameSettingsInstaller.ControlSettings _controlSettings;
    private readonly CompositeDisposable _compositeDisposable = new();

    [Inject]
    public LookTuner(SettingsStorage settingsStorage, GameSettingsInstaller.ControlSettings controlSettings)
    {
        _settingsStorage = settingsStorage;
        _controlSettings = controlSettings;
    }

    public ReactiveProperty<float> Sensitivity => _sensitivity;

    public void Initialize()
    {
        _sensitivity.Value = _settingsStorage.Get(SettingsConstants.SensitivityKey, _controlSettings.DefaultSensitivity);

        Sensitivity
            .Subscribe(value => 
            _sensitivity.Value = Math.Clamp(value, _controlSettings.MinSensitivity, _controlSettings.MaxSensitivity))
            .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _settingsStorage.Set(SettingsConstants.SensitivityKey, _sensitivity.Value);

        _compositeDisposable?.Dispose();
    }
}
