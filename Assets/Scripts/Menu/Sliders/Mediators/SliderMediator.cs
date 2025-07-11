using R3;

public class SliderMediator : Mediator
{
    private readonly SliderModel _sliderModel;
    private readonly SliderView _sliderView;

    public SliderMediator(SliderModel sliderModel, SliderView sliderView)
    {
        _sliderModel = sliderModel;
        _sliderView = sliderView;
    }

    public override void Initialize()
    {
        _sliderView.SetMinValue(_sliderModel.MinValue);
        _sliderView.SetMaxValue(_sliderModel.MaxValue);

        _sliderModel.Value
            .Subscribe(_sliderView.SetValueWithoutNotify)
            .AddTo(CompositeDisposable);

        _sliderView.Value
            .Subscribe(_sliderModel.SetClampedValue)
            .AddTo(CompositeDisposable);
    }
}
