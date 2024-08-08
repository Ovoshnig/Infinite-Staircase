using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public abstract class DataSaver : IDisposable
{
    private readonly Dictionary<string, object> _defaultDataStore = new();
    private Dictionary<string, object> _dataStore;

    public DataSaver() => LoadDataStore();

    protected virtual string SaveFileName { get; } = string.Empty;
    protected string FilePath => Path.Combine(Application.persistentDataPath, SaveFileName);
    protected Dictionary<string, object> DataStore => _dataStore;

    public void Dispose() => SaveDataStore();

    public T LoadData<T>(string key, T defaultValue)
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

    public void SaveData<T>(string key, T value) => _dataStore[key] = value;

    public void ResetData()
    {
        foreach (var key in _defaultDataStore.Keys)
            DataStore[key] = _defaultDataStore[key];
    }

    private void LoadDataStore()
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            _dataStore = JsonConvert.DeserializeObject<Dictionary<string, object>>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
        else
        {
            _dataStore = new Dictionary<string, object>();
        }
    }

    private void SaveDataStore()
    {
        string json = JsonConvert.SerializeObject(_dataStore, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
        File.WriteAllText(FilePath, json);
    }
}
