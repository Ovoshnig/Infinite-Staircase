using R3;

public abstract class SliderDataKeeperMediator : Mediator
{
    private readonly SliderView _sliderView;
    private readonly DataKeeper<float> _dataKeeper;

    public SliderDataKeeperMediator(SliderView sliderView,
        DataKeeper<float> dataKeeper)
    {
        _sliderView = sliderView;
        _dataKeeper = dataKeeper;
    }

    protected abstract float MinValue { get; }
    protected abstract float MaxValue { get; }

    public override void Initialize()
    {
        _sliderView.SetMinValue(MinValue);
        _sliderView.SetMaxValue(MaxValue);

        _dataKeeper.Data
            .Subscribe(_sliderView.SetValue)
            .AddTo(CompositeDisposable);

        _sliderView.Value
            .Skip(1)
            .Subscribe(_dataKeeper.SetValue)
            .AddTo(CompositeDisposable);
    }
}
