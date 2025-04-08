using R3;
using System;
using VContainer.Unity;

public abstract class DataKeeper<T> : IPostInitializable, IDisposable
{
    private readonly ReactiveProperty<T> _data = new();
    private readonly DataStorage _dataStorage;

    public DataKeeper(DataStorage dataStorage) => _dataStorage = dataStorage;

    public ReadOnlyReactiveProperty<T> Data => _data;
    protected abstract string DataKey { get; }
    protected abstract T DefaultValue { get; }

    public void PostInitialize()
    {
        T value = _dataStorage.Get(DataKey, DefaultValue);
        SetValue(value);
    }

    public void Dispose() =>
        _dataStorage.Set(DataKey, _data.Value);

    public virtual void SetValue(T value) => _data.Value = value;
}
