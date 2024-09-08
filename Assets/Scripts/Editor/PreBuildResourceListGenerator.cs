using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Build;
using UnityEngine;

public class PreBuildMusicListGenerator : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report) => GenerateMusicClipLists();

    private void GenerateMusicClipLists()
    {
        foreach (MusicCategory category in Enum.GetValues(typeof(MusicCategory)))
        {
            string categoryPath = $"{ResourcesConstants.ResourcesPath}/{ResourcesConstants.MusicPath}/{category}";

            if (!Directory.Exists(categoryPath))
            {
                Debug.LogWarning($"Folder {categoryPath} not found, skipping {category} category.");
                continue;
            }

            string[] filePaths = Directory.GetFiles(categoryPath, "*.mp3", SearchOption.TopDirectoryOnly);

            List<string> clipNames = new();

            foreach (string filePath in filePaths)
            {
                string relativePath = filePath
                    .Replace(ResourcesConstants.ResourcesPath + '/', "")
                    .Replace('\\', '/')
                    .Replace(Path.GetExtension(filePath), "");

                clipNames.Add(relativePath);
            }

            string listFilePath = $"{categoryPath}/clipList.txt";

            using (StreamWriter writer = new(listFilePath))
            {
                foreach (var clipName in clipNames)
                    writer.WriteLine(clipName);
            }

            Debug.Log($"Список клипов для категории {category} сгенерирован и сохранен в {listFilePath}.");
        }

        AssetDatabase.Refresh();
    }
}
