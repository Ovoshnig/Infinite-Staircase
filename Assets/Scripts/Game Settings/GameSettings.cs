using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Scriptable Objects/Settings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private TimeSettings _timeSettings;
    [SerializeField] private ControlSettings _controlSettings;
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private AudioSettings _audioSettings;
    [SerializeField] private WorldGenerationSettings _worldGeneration;
    [SerializeField] private StaircaseGenerationSettings _staircaseGeneration;
    [SerializeField] private PlayerSettings _playerSettings;
    [SerializeField] private InventorySettings _inventorySettings;

    public TimeSettings TimeSettings => _timeSettings;
    public ControlSettings ControlSettings => _controlSettings;
    public LevelSettings LevelSettings => _levelSettings;
    public AudioSettings AudioSettings => _audioSettings;
    public WorldGenerationSettings WorldGeneration => _worldGeneration;
    public StaircaseGenerationSettings StaircaseGeneration => _staircaseGeneration;
    public PlayerSettings PlayerSettings => _playerSettings;
    public InventorySettings InventorySettings => _inventorySettings;
}
