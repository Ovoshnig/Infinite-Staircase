using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameSettings),
    menuName = "Scriptable Objects/Game Settings")]
public class GameSettings : ScriptableObject
{
    [field: SerializeField] public TimeSettings TimeSettings { get; private set; }
    [field: SerializeField] public SceneSettings SceneSettings { get; private set; }
    [field: SerializeField] public AudioSettings AudioSettings { get; private set; }
    [field: SerializeField] public WorldGenerationSettings WorldGeneration { get; private set; }
    [field: SerializeField] public StaircaseGenerationSettings StaircaseGeneration { get; private set; }
    [field: SerializeField] public GlassFloorSettings GlassFloorSettings { get; private set; }
    [field: SerializeField] public PlayerSettings PlayerSettings { get; private set; }
    [field: SerializeField] public KeyBindingSettings KeyBindingSettings { get; private set; }
    [field: SerializeField] public InventorySettings InventorySettings { get; private set; }
}
