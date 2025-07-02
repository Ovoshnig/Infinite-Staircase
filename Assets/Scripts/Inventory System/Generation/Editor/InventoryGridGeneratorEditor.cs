using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(InventoryGridGenerator))]
public class InventoryGridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InventoryGridGenerator generator = (InventoryGridGenerator)target;

        if (GUILayout.Button("Generate Inventory Grid"))
        {
            if (!Application.isPlaying)
            {
                int group = Undo.GetCurrentGroup();
                Undo.IncrementCurrentGroup();
                Undo.SetCurrentGroupName("Generate Inventory Grid");

                Undo.RegisterFullObjectHierarchyUndo(generator.ContainerObject, "Generate Inventory Grid");

                generator.GenerateGrid();

                EditorUtility.SetDirty(generator.ContainerObject);
                EditorSceneManager.MarkSceneDirty(generator.ContainerObject.scene);
            }
            else
            {
                Debug.LogWarning("Slot generation is disabled during Play Mode.");
            }
        }
    }
}
