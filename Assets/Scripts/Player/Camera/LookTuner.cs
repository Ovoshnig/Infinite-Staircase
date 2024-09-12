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
        _sensitivity.Value = _settingsStorage.Get(SettingsConstants.SensitivityKey, _controlSettings.DefaultSensitivity);

        _disposable = Sensitivity
            .Subscribe(value =>
            {
                value = Math.Clamp(value, _controlSettings.MinSensitivity, _controlSettings.MaxSensitivity);
                _sensitivity.Value = value;
                /*var stringValue = value.ToString().Replace(',', '.');

                InputSystem.actions.FindActionMap("Player")
                .FindAction("Look")
                .ApplyBindingOverride(new InputBinding()
                {
                    overridePath = "<Mouse>/delta",
                    overrideProcessors = $"scaleVector2(x={stringValue}, y={stringValue})"
                });*/
            });
    }

    public void Dispose()
    {
        _settingsStorage.Set(SettingsConstants.SensitivityKey, _sensitivity.Value);

        _disposable?.Dispose();
    }
}
