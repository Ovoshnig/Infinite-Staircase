using System.Collections.Generic;

public sealed class SaveSaver : DataSaver
{
    private readonly Dictionary<string, object> _defaultStore = new();

    public override T LoadData<T>(string key, T defaultValue)
    {
        _defaultStore[key] = defaultValue;

        return base.LoadData(key, defaultValue);
    }

    public void Reset()
    {
        foreach(var key in _defaultStore.Keys)
            DataStore[key] = _defaultStore[key];
    }
    protected override void SetFileName() => SaveFileName = SaveConstants.SaveFileName;
}
