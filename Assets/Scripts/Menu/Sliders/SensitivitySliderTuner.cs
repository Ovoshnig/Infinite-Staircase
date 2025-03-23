using R3;
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
        Slider.minValue = _controlSettings.MinSensitivity;
        Slider.maxValue = _controlSettings.MaxSensitivity;

        _lookTuner.Sensitivity
            .Subscribe(Slider.SetValueWithoutNotify)
            .AddTo(this);

        base.Start();
    }

    protected override void OnSliderValueChanged(float value) => _lookTuner.SetSensitivity(value);
}
