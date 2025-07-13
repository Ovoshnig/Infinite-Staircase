using R3;

public class SensitivitySliderInputAxisControllerMediator : Mediator
{
    private readonly SensitivitySliderModel _sensitivitySliderModel;
    private readonly InputAxisView _inputAxisController;

    public SensitivitySliderInputAxisControllerMediator(SensitivitySliderModel sensitivitySliderModel, 
        InputAxisView inputAxisController)
    {
        _sensitivitySliderModel = sensitivitySliderModel;
        _inputAxisController = inputAxisController;
    }

    public override void Initialize()
    {
        _sensitivitySliderModel.Value
            .Subscribe(_inputAxisController.SetControllersMultiplier)
            .AddTo(CompositeDisposable);
    }
}
