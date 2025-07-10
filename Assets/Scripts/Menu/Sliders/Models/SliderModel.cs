using R3;
using System;
using VContainer.Unity;

public abstract class SliderModel : IInitializable, IDisposable
{
    private readonly ReactiveProperty<float> _value = new();
    private readonly DataStorage _dataStorage;

    public SliderModel(DataStorage dataStorage) => _dataStorage = dataStorage;

    public ReadOnlyReactiveProperty<float> Value => _value;
    public abstract float MinValue { get; }
    public abstract float MaxValue { get; }

    protected abstract string DataKey { get; }
    protected abstract float DefaultValue { get; }

    public void Initialize()
    {
        float value = _dataStorage.Get(DataKey, DefaultValue);
        SetClampedValue(value);
    }

    public void Dispose() =>
        _dataStorage.Set(DataKey, _value.Value);

    public void SetClampedValue(float value)
    {
        float clampedValue = Math.Clamp(value, MinValue, MaxValue);
        _value.Value = clampedValue;
    }
}
