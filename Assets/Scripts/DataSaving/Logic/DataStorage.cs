using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using R3;
using UnityEngine;
using VContainer.Unity;

public abstract class DataStorage : IInitializable, IDisposable
{
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
    private readonly Dictionary<string, object> _defaultDataStore = new();
    private readonly Dictionary<string, object> _runtimeCache = new();
    private readonly Subject<Unit> _resetHappened = new();

    private Dictionary<string, JsonElement> _rawData = new();

    public Observable<Unit> ResetHappened => _resetHappened;

    protected abstract string SaveFileName { get; }
    protected string FilePath => Path.Combine(Application.persistentDataPath, SaveFileName);

    public void Initialize() => LoadData();

    public void Dispose()
    {
        SaveData();
        _resetHappened.Dispose();
    }

    public virtual T Get<T>(string key, T defaultValue)
    {
        _defaultDataStore[key] = defaultValue;

        if (_runtimeCache.TryGetValue(key, out object cachedValue))
            return (T)cachedValue;

        if (_rawData.TryGetValue(key, out JsonElement jsonElement))
        {
            try
            {
                T value = jsonElement.Deserialize<T>(_jsonOptions);
                _runtimeCache[key] = value;
                return value;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to deserialize key {key}: {e.Message}");
            }
        }

        _runtimeCache[key] = defaultValue;
        return defaultValue;
    }

    public virtual void Set<T>(string key, T value) => _runtimeCache[key] = value;

    public virtual void ResetData()
    {
        _runtimeCache.Clear();
        _rawData.Clear();

        foreach (var kvp in _defaultDataStore)
            _runtimeCache[kvp.Key] = kvp.Value;

        _resetHappened.OnNext(Unit.Default);
    }

    protected virtual void LoadData()
    {
        if (!File.Exists(FilePath))
            return;

        string json = File.ReadAllText(FilePath);
        _rawData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json, _jsonOptions) ?? new();
    }

    protected virtual void SaveData()
    {
        foreach (var kvp in _runtimeCache)
            _rawData[kvp.Key] = JsonSerializer.SerializeToElement(kvp.Value, kvp.Value.GetType(), _jsonOptions);

        string json = JsonSerializer.Serialize(_rawData, _jsonOptions);
        File.WriteAllText(FilePath, json);
    }
}
