using R3;
using System;
using VContainer.Unity;

public abstract class SliderDataKeeperMediator : IPostInitializable, IDisposable
{
    private readonly SliderView _sliderView;
    private readonly DataKeeper<float> _dataKeeper;
    private readonly CompositeDisposable _compositeDisposable = new();

    public SliderDataKeeperMediator(SliderView sliderView,
        DataKeeper<float> dataKeeper)
    {
        _sliderView = sliderView;
        _dataKeeper = dataKeeper;;
    }

    protected abstract float MinValue { get; }
    protected abstract float MaxValue { get; }

    public void PostInitialize()
    {
        _sliderView.Awake();
        _sliderView.SetMinValue(MinValue);
        _sliderView.SetMaxValue(MaxValue);

        _dataKeeper.Data
            .Subscribe(_sliderView.SetValue)
            .AddTo(_compositeDisposable);
        _sliderView.Value
            .Skip(1)
            .Subscribe(_dataKeeper.SetValue)
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
