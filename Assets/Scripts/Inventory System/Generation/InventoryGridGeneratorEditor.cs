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
            Undo.RegisterFullObjectHierarchyUndo(generator.gameObject, "Generate Inventory Grid");
            generator.TryGenerate();
            EditorUtility.SetDirty(generator.gameObject);
            EditorSceneManager.MarkSceneDirty(generator.gameObject.scene);
        }
    }
}
