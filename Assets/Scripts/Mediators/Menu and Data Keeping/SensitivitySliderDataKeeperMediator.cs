public class SensitivitySliderDataKeeperMediator : SliderDataKeeperMediator
{
    private readonly PlayerSettings _playerSettings;

    public SensitivitySliderDataKeeperMediator(SensitivitySliderView sensitivitySliderView,
        SensitivityKeeper sensitivityKeeper, PlayerSettings playerSettings)
        : base(sensitivitySliderView, sensitivityKeeper) => _playerSettings = playerSettings;

    protected override float MinValue => _playerSettings.MinSensitivity;
    protected override float MaxValue => _playerSettings.MaxSensitivity;
}
