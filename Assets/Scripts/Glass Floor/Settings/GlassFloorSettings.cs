using UnityEngine;

[CreateAssetMenu(fileName = "FloorSettings", menuName = "Scriptable Objects/Floor Settings")]
public class GlassFloorSettings : ScriptableObject
{
    [field: SerializeField, Min(1)] public int Length { get; private set; } = 10;
    [field: SerializeField, Min(1)] public int Width { get; private set; } = 10;
    [field: SerializeField, Range(0.1f, 2f)] public float Height { get; private set; } = 1f;
    [field: SerializeField, Range(0.5f, 1f)] public float ColliderResolution { get; private set; } = 1f;
}
