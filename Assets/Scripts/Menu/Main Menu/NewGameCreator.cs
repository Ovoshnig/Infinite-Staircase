using Cysharp.Threading.Tasks;
using Random = System.Random;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class NewGameCreator : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private TMP_InputField _seedInputField;

    private readonly Random _random = new();
    private SceneSwitch _sceneSwitch;
    private SaveStorage _saveStorage;
    private GameSettingsInstaller.WorldGenerationSettings _generationSettings;

    [Inject]
    private void Construct(SceneSwitch sceneSwitch, SaveStorage saveStorage,
        GameSettingsInstaller.WorldGenerationSettings generationSettings)
    {
        _sceneSwitch = sceneSwitch;
        _saveStorage = saveStorage;
        _generationSettings = generationSettings;
    }

    private void Start() => _seedInputField.contentType = TMP_InputField.ContentType.IntegerNumber;

    private void OnEnable() => _startGameButton.onClick.AddListener(OnStartGameButtonClicked);

    private void OnDisable() => _startGameButton.onClick.RemoveListener(OnStartGameButtonClicked);

    private void OnStartGameButtonClicked()
    {
        int seed;

        if (int.TryParse(_seedInputField.text, out int value))
            seed = value; 
        else
            seed = _random.Next(_generationSettings.MinSeed, _generationSettings.MaxSeed);

        _saveStorage.Set(SaveConstants.SaveCreatedKey, true);
        _saveStorage.Set(SaveConstants.SeedKey, seed);
        _sceneSwitch.LoadFirstLevel().Forget();
    }
}
