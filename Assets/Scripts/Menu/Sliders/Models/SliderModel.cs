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
        SetValue(value);
    }

    public void Dispose() =>
        _dataStorage.Set(DataKey, _value.Value);

    public virtual void SetValue(float value) => _value.Value = value;
}
