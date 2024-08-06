using System.Collections.Generic;
using System.IO;

public sealed class SaveSaver : DataSaver
{
    private readonly Dictionary<string, object> _defaultStore = new();

    public override T LoadData<T>(string key, T defaultValue = default)
    {
        _defaultStore[key] = defaultValue;

        return base.LoadData(key, defaultValue);
    }

    protected override void SetFileName() => SaveFileName = "save.json";

    public void Reset()
    {
        if (File.Exists(FilePath))
            File.WriteAllText(SaveFileName, string.Empty);

        foreach(var key in _defaultStore.Keys)
            DataStore[key] = _defaultStore[key];
    }
}
