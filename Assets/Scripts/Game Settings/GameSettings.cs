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
    [SerializeField] private PlayerSettings _playerSettings;

    public TimeSettings TimeSettings => _timeSettings;
    public SceneSettings SceneSettings => _sceneSettings;
    public AudioSettings AudioSettings => _audioSettings;
    public WorldGenerationSettings WorldGeneration => _worldGeneration;
    public StaircaseGenerationSettings StaircaseGeneration => _staircaseGeneration;
    public PlayerSettings PlayerSettings => _playerSettings;
}
