using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryGridGenerator))]
public class InventoryGridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InventoryGridGenerator generator = (InventoryGridGenerator)target;

        if (GUILayout.Button("Generate Inventory Grid"))
            generator.TryGenerate();
    }
}
