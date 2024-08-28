using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public abstract class DataStorage : IDisposable
{
    private readonly Dictionary<string, object> _defaultDataStore = new();
    private Dictionary<string, object> _dataStore;

    public DataStorage() => LoadData();

    protected virtual string SaveFileName => string.Empty;
    protected string FilePath => Path.Combine(Application.persistentDataPath, SaveFileName);
    protected IReadOnlyDictionary<string, object> DataStore => _dataStore;
    protected JsonSerializerSettings JsonSerializerSettings => new()
    {
        TypeNameHandling = TypeNameHandling.Auto
    };

    public void Dispose() => SaveData();

    public T Get<T>(string key, T defaultValue)
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

    public void Set<T>(string key, T value) => _dataStore[key] = value;

    public void ResetData()
    {
        foreach (var key in _defaultDataStore.Keys)
        {
            if (_defaultDataStore.TryGetValue(key, out object value))
                _dataStore[key] = value;
            else
                _dataStore[key] = default;
        }
    }

    protected void ResetData(Dictionary<string, object> backupDataStore)
    {
        foreach(var key in backupDataStore.Keys)
        {
            if (backupDataStore.TryGetValue(key, out object value))
                _dataStore[key] = value;
            else 
                _dataStore[key] = default;
        }
    }

    protected virtual void LoadData()
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            _dataStore = JsonConvert.DeserializeObject<Dictionary<string, object>>(json, JsonSerializerSettings);
        }
        else
        {
            _dataStore = new Dictionary<string, object>();
        }
    }

    protected virtual void SaveData()
    {
        string json = JsonConvert.SerializeObject(_dataStore, Formatting.Indented, JsonSerializerSettings);
        File.WriteAllText(FilePath, json);
    }
}
