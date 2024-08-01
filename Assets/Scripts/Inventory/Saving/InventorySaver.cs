using System;
using System.IO;
using UnityEngine;

public class InventorySaver : IDisposable
{
    private const string SaveFileName = "inventoryData.json";

    public InventorySaver() => LoadDataStore();

    private string FilePath => Path.Combine(Application.persistentDataPath, SaveFileName);

    public void Dispose() => SaveDataStore();

    private void LoadDataStore()
    {
        
    }

    private void SaveDataStore()
    {

    }
}
