public class SensitivitySliderInputAxisViewMediatorFactory
    : MediatorViewFactory<SensitivitySliderInputAxisViewMediator, InputAxisView>
{
    private readonly SensitivitySliderModel _sensitivitySliderModel;

    public SensitivitySliderInputAxisViewMediatorFactory(SensitivitySliderModel sensitivitySliderModel) =>
        _sensitivitySliderModel = sensitivitySliderModel;

    protected override SensitivitySliderInputAxisViewMediator CreateMediatorInstance(InputAxisView view) =>
        new(_sensitivitySliderModel, view);
}
