using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(GlassFloorGeneratorView))]
public class GlassFloorGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GlassFloorGeneratorView generator = (GlassFloorGeneratorView)target;

        if (GUILayout.Button("Generate Floor"))
        {
            if (Application.isPlaying)
            {
                Debug.LogWarning("Floor generation is disabled during Play Mode.");
            }
            else
            {
                int group = Undo.GetCurrentGroup();
                Undo.IncrementCurrentGroup();
                Undo.SetCurrentGroupName("Generate Floor");

                Undo.RegisterFullObjectHierarchyUndo(generator.gameObject, "Generate Floor");

                generator.GenerateInEditor();

                EditorUtility.SetDirty(generator.gameObject);
                EditorSceneManager.MarkSceneDirty(generator.gameObject.scene);
            }
        }
    }
}
