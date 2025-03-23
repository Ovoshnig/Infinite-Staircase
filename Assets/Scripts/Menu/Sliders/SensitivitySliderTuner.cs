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

    protected override void Start()
    {
        base.Start();

        Slider.minValue = _controlSettings.MinSensitivity;
        Slider.maxValue = _controlSettings.MaxSensitivity;
        Slider.SetValueWithoutNotify(_lookTuner.Sensitivity.CurrentValue);
    }

    protected override void OnSliderValueChanged(float value) => _lookTuner.SetSensitivity(value);
}
