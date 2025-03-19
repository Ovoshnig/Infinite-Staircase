using VContainer;

public sealed class SensitivitySliderTuner : SliderTuner
{
    private LookTuner _lookTuner;
    private ControlSettings _controlSettings;

    [Inject]
    public void Construct(LookTuner lookTuner, 
        ControlSettings controlSettings)
    {
        _lookTuner = lookTuner;
        _controlSettings = controlSettings;
    }

    protected override float MinValue => _controlSettings.MinSensitivity;

    protected override float MaxValue => _controlSettings.MaxSensitivity;

    protected override float InitialValue => _lookTuner.Sensitivity.Value;

    protected override void OnSliderValueChanged(float value) => _lookTuner.Sensitivity.Value = value;
}
