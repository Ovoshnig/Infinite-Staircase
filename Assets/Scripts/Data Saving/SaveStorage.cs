using Cryptography = System.Security.Cryptography;
using System;
using System.IO;
using UnityEngine;

public class SaveStorage : DataStorage
{
    protected override string SaveFileName => SaveConstants.SaveFileName;

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
        File.WriteAllText(HashFilePath, fileHash);
    }

    private string CalculateHash(string filePath)
    {
        using var sha256 = Cryptography.SHA256.Create();
        byte[] fileBytes = File.ReadAllBytes(filePath);
        byte[] hashBytes = sha256.ComputeHash(fileBytes);

        return Convert.ToBase64String(hashBytes);
    }
}
