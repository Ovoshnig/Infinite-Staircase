using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using R3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using VContainer.Unity;

public abstract class DataStorage : IInitializable, IDisposable
{
    private readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto
    };
    private readonly Dictionary<string, object> _defaultDataStore = new();
    private readonly Subject<Unit> _resetHappened = new();

    private Dictionary<string, object> _dataStore = new();

    public Observable<Unit> ResetHappened => _resetHappened;

    protected abstract string SaveFileName { get; }
    protected string FilePath => Path.Combine(Application.persistentDataPath, SaveFileName);
    protected IReadOnlyDictionary<string, object> DataStore => _dataStore;

    public void Initialize() => LoadData();

    public void Dispose() => SaveData();

    public virtual T Get<T>(string key, T defaultValue)
    {
        _defaultDataStore[key] = defaultValue;

        if (_dataStore.TryGetValue(key, out object storedValue))
        {
            try
            {
                return storedValue is JObject jObject
                    ? jObject.ToObject<T>()
                    : (T)Convert.ChangeType(storedValue, typeof(T));
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to deserialize value for key {key}: {e.Message}");
                return defaultValue;
            }
        }

        return defaultValue;
    }

    public virtual void Set<T>(string key, T value) => _dataStore[key] = value;

    public virtual void ResetData()
    {
        List<string> keys = _dataStore.Keys.ToList();

        foreach (var key in keys)
        {
            if (_defaultDataStore.TryGetValue(key, out object value))
            {
                _dataStore[key] = value;
            }
            else
            {
                object existingValue = _dataStore[key];
                Type type = existingValue?.GetType();

                _dataStore[key] = type != null
                    ? (type.IsValueType ? Activator.CreateInstance(type) : null)
                    : null;
            }
        }

        _resetHappened.OnNext(Unit.Default);
    }

    protected virtual void LoadData()
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            _dataStore = JsonConvert.DeserializeObject<Dictionary<string, object>>(json, _jsonSerializerSettings);
        }
    }

    protected virtual void SaveData()
    {
        string json = JsonConvert.SerializeObject(_dataStore, Formatting.Indented, _jsonSerializerSettings);
        File.WriteAllText(FilePath, json);
    }
}
