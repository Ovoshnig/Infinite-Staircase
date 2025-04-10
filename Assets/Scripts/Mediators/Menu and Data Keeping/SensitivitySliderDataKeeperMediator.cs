public class SensitivitySliderDataKeeperMediator : SliderDataKeeperMediator
{
    private readonly ControlSettings _controlSettings;

    public SensitivitySliderDataKeeperMediator(SensitivitySliderView sensitivitySliderView,
        SensitivityKeeper sensitivityKeeper, ControlSettings controlSettings)
        : base(sensitivitySliderView, sensitivityKeeper) => _controlSettings = controlSettings;

    protected override float MinValue => _controlSettings.MinSensitivity;
    protected override float MaxValue => _controlSettings.MaxSensitivity;
}
