using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class DataSaver : IDisposable
{
    private const string SaveFileName = "playerData.json";

    private Dictionary<string, object> _dataStore;

    private string FilePath => Path.Combine(Application.persistentDataPath, SaveFileName);

    public DataSaver() => LoadDataStore();

    public void Dispose() => SaveDataStore();

    public T LoadData<T>(string key, T defaultValue = default)
    {
        if (_dataStore.TryGetValue(key, out object storedValue))
        {
            try
            {
                if (typeof(T) == typeof(float) && storedValue is double doubleValue)
                    return (T)(object)(float)doubleValue;

                return JsonConvert.DeserializeObject<T>(storedValue.ToString());
            }
            catch (JsonException)
            {
                Debug.LogWarning($"Failed to deserialize value for key {key}");

                return defaultValue;
            }
        }

        return defaultValue;
    }

    public void SaveData<T>(string key, T value) => _dataStore[key] = value;

    private void LoadDataStore()
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            _dataStore = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }
        else
        {
            _dataStore = new Dictionary<string, object>();
        }
    }

    private void SaveDataStore()
    {
        string json = JsonConvert.SerializeObject(_dataStore, Formatting.Indented);
        File.WriteAllText(FilePath, json);
    }
}
