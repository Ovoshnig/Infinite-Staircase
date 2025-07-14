using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameSettings),
    menuName = "Scriptable Objects/Game Settings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private TimeSettings _timeSettings;
    [SerializeField] private SceneSettings _sceneSettings;
    [SerializeField] private AudioSettings _audioSettings;
    [SerializeField] private WorldGenerationSettings _worldGeneration;
    [SerializeField] private StaircaseGenerationSettings _staircaseGeneration;
    [SerializeField] private GlassFloorSettings _glassFloorSettings;
    [SerializeField] private PlayerSettings _playerSettings;
    [SerializeField] private KeyBindingSettings _keyBindingSettings;
    [SerializeField] private InventorySettings _inventorySettings;
         
    public TimeSettings TimeSettings => _timeSettings;
    public SceneSettings SceneSettings => _sceneSettings;
    public AudioSettings AudioSettings => _audioSettings;
    public WorldGenerationSettings WorldGeneration => _worldGeneration;
    public StaircaseGenerationSettings StaircaseGeneration => _staircaseGeneration;
    public GlassFloorSettings GlassFloorSettings => _glassFloorSettings;
    public PlayerSettings PlayerSettings => _playerSettings;
    public KeyBindingSettings KeyBindingSettings => _keyBindingSettings;
    public InventorySettings InventorySettings => _inventorySettings;
}
