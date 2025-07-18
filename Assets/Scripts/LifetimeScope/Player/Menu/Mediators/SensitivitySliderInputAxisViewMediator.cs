using R3;

public class SensitivitySliderInputAxisViewMediator : Mediator
{
    private readonly SensitivitySliderModel _sensitivitySliderModel;
    private readonly InputAxisView _inputAxisView;

    public SensitivitySliderInputAxisViewMediator(SensitivitySliderModel sensitivitySliderModel,
        InputAxisView inputAxisView)
    {
        _sensitivitySliderModel = sensitivitySliderModel;
        _inputAxisView = inputAxisView;
    }

    public override void Initialize()
    {
        _sensitivitySliderModel.Value
            .Subscribe(_inputAxisView.SetLookControllersGain)
            .AddTo(CompositeDisposable);
    }
}
