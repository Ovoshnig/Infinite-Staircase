using Zenject;

public sealed class SensitivitySliderTuner : SliderTuner
{
    private LookTuner _lookTuner;
    private GameSettingsInstaller.ControlSettings _controlSettings;

    [Inject]
    private void Construct(LookTuner lookTuner, 
        GameSettingsInstaller.ControlSettings controlSettings)
    {
        _lookTuner = lookTuner;
        _controlSettings = controlSettings;
    }

    protected override float MinValue => _controlSettings.MinSensitivity;

    protected override float MaxValue => _controlSettings.MaxSensitivity;

    protected override float InitialValue => _lookTuner.Sensitivity.CurrentValue;

    protected override void OnSliderValueChanged(float value) => _lookTuner.Sensitivity.Value = value;
}
