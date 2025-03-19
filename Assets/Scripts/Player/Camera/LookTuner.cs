using R3;
using System;
using VContainer.Unity;

public class LookTuner : IPostInitializable, IDisposable
{
    private readonly ReactiveProperty<float> _sensitivity = new(0f);
    private readonly SettingsStorage _settingsStorage;
    private readonly ControlSettings _controlSettings;
    private readonly CompositeDisposable _compositeDisposable = new();

    public LookTuner(SettingsStorage settingsStorage, ControlSettings controlSettings)
    {
        _settingsStorage = settingsStorage;
        _controlSettings = controlSettings;
    }

    public ReactiveProperty<float> Sensitivity => _sensitivity;

    public void PostInitialize()
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
