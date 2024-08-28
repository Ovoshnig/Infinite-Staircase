using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class SaveStorage : DataStorage
{
    protected override string SaveFileName => SaveConstants.SaveFileName;

    private string BackupFilePath => Path.Combine(Application.persistentDataPath, SaveConstants.BackupFileName);
    private string HashFilePath => Path.Combine(Application.persistentDataPath, SaveConstants.HashFileName);

    protected override void LoadData()
    {
        base.LoadData();

        if (File.Exists(FilePath) && File.Exists(HashFilePath))
        {
            string savedHash = File.ReadAllText(HashFilePath);
            string currentHash = CalculateHash(FilePath);

            if (savedHash != currentHash)
            {
                Debug.LogWarning("File integrity check failed. The save file might have been tampered with.");

                if (File.Exists(BackupFilePath))
                    LoadFromBackup();
                else
                    ResetData();
            }
        }
        else
        {
            ResetData();
        }
    }

    protected override void SaveData()
    {
        base.SaveData();

        string fileHash = CalculateHash(FilePath);

        SaveToFile(HashFilePath, fileHash);
        SaveToFile(BackupFilePath, JsonConvert.SerializeObject(DataStore, Formatting.Indented, JsonSerializerSettings));
    }

    private void SaveToFile(string filePath, string content)
    {
        if (File.Exists(filePath))
        {
            RemoveFileAttributes(filePath, FileAttributes.ReadOnly | FileAttributes.Hidden);
            File.Delete(filePath);
        }

        File.WriteAllText(filePath, content);
        File.SetAttributes(filePath, FileAttributes.ReadOnly | FileAttributes.Hidden);
    }

    private void LoadFromBackup()
    {
        string json = File.ReadAllText(BackupFilePath);
        var backupDataStore = JsonConvert.DeserializeObject<Dictionary<string, object>>(json, JsonSerializerSettings);
        ResetData(backupDataStore);
    }

    private void RemoveFileAttributes(string filePath, FileAttributes attributes)
    {
        FileAttributes currentAttributes = File.GetAttributes(filePath);

        if ((currentAttributes & attributes) != 0)
            File.SetAttributes(filePath, currentAttributes & ~attributes);
    }

    private string CalculateHash(string filePath)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        byte[] fileBytes = File.ReadAllBytes(filePath);
        byte[] hashBytes = sha256.ComputeHash(fileBytes);

        return Convert.ToBase64String(hashBytes);
    }
}
