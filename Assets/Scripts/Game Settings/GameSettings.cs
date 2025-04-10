using UnityEngine;

[CreateAssetMenu(fileName = GameSettingsConstants.FileName, 
    menuName = GameSettingsConstants.MenuName)]
public class GameSettings : ScriptableObject
{
    [SerializeField] private TimeSettings _timeSettings;
    [SerializeField] private SceneSettings _sceneSettings;
    [SerializeField] private AudioSettings _audioSettings;
    [SerializeField] private WorldGenerationSettings _worldGeneration;
    [SerializeField] private StaircaseGenerationSettings _staircaseGeneration;
    [SerializeField] private PlayerSettings _playerSettings;
    [SerializeField] private InventorySettings _inventorySettings;

    public TimeSettings TimeSettings => _timeSettings;
    public SceneSettings SceneSettings => _sceneSettings;
    public AudioSettings AudioSettings => _audioSettings;
    public WorldGenerationSettings WorldGeneration => _worldGeneration;
    public StaircaseGenerationSettings StaircaseGeneration => _staircaseGeneration;
    public PlayerSettings PlayerSettings => _playerSettings;
    public InventorySettings InventorySettings => _inventorySettings;
}
