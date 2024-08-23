using System;
using System.IO;
using UnityEngine;

public sealed class SaveSaver : DataSaver
{
    protected override string SaveFileName { get; } = SaveConstants.SaveFileName;
    private string HashFilePath => Path.Combine(Application.persistentDataPath, SaveConstants.HashFileName);

    public SaveSaver() : base() => VerifyFileIntegrity();

    private void VerifyFileIntegrity()
    {
        if (File.Exists(FilePath) && File.Exists(HashFilePath))
        {
            string savedHash = File.ReadAllText(HashFilePath);
            string currentHash = CalculateHash(FilePath);

            if (savedHash != currentHash)
            {
                Debug.LogWarning("File integrity check failed. The save file might have been tampered with.");
                // Действия при несоответствии хеша
            }
            else
            {
                Debug.Log("Loading correct");
            }
        }
    }

    private string CalculateHash(string filePath)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        byte[] fileBytes = File.ReadAllBytes(filePath);
        byte[] hashBytes = sha256.ComputeHash(fileBytes);

        return Convert.ToBase64String(hashBytes);
    }

    protected override void SaveDataStore()
    {
        base.SaveDataStore();

        string fileHash = CalculateHash(FilePath);
        File.WriteAllText(HashFilePath, fileHash);
    }
}
