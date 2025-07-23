using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(KeyBindingsGenerator))]
public class KeyBindingsGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        KeyBindingsGenerator generator = (KeyBindingsGenerator)target;

        if (GUILayout.Button("Generate Key Bindings"))
        {
            if (Application.isPlaying)
            {
                Debug.LogWarning("Key bindings generation is disabled during Play Mode.");
                return;
            }

            int group = Undo.GetCurrentGroup();
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Generate Key Bindings");

            Undo.RegisterFullObjectHierarchyUndo(generator.gameObject, "Generate Key Bindings");

            generator.GenerateBindings();

            EditorUtility.SetDirty(generator.gameObject);
            EditorSceneManager.MarkSceneDirty(generator.gameObject.scene);
        }
    }
}
