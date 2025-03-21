using System.IO;
using UnityEditor;

public class ResourcesPathsListCreator
{
    [MenuItem("Assets/Create list of resources paths")]
    public static void CreateList()
    {
        string[] guids = AssetDatabase.FindAssets("", new[] { ResourcesConstants.ResourcesPath });

        string listFilePath = $"{ResourcesConstants.ResourcesPath}/{ResourcesConstants.ResourcesListName}.txt";
        using StreamWriter writer = new(listFilePath);

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string resourcePath = path.Replace($"{ResourcesConstants.ResourcesPath}/", "");

            if (string.IsNullOrEmpty(Path.GetExtension(resourcePath)))
                continue;

            writer.WriteLine(resourcePath);
        }

        AssetDatabase.Refresh();
    }
}
