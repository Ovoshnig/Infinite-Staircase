using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public sealed class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _continueGameButton;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _quitGameButton;
    [SerializeField] private GameObject _resetWarningPanel;
    [SerializeField] private GameObject _gameCreationPanel;

    private SaveStorage _saveStorage;
    private SceneSwitch _sceneSwitch;

    [Inject]
    private void Construct(SaveStorage saveStorage, SceneSwitch sceneSwitch)
    {
        _saveStorage = saveStorage;
        _sceneSwitch = sceneSwitch;
    }

    private void OnEnable()
    {
        _continueGameButton.interactable = _saveStorage.Get(SaveConstants.SaveCreatedKey, false);

        _continueGameButton.onClick.AddListener(OnContinueGameButtonClicked);
        _newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        _quitGameButton.onClick.AddListener(OnQuitGameButtonClicked);
    }

    private void OnDisable()
    {
        _continueGameButton.onClick.RemoveListener(OnContinueGameButtonClicked);
        _newGameButton.onClick.RemoveListener(OnNewGameButtonClicked);
        _quitGameButton.onClick.RemoveListener(OnQuitGameButtonClicked);
    }

    private void OnContinueGameButtonClicked() => _sceneSwitch.LoadAchievedLevel().Forget();

    private void OnNewGameButtonClicked()
    {
        if (_saveStorage.Get(SaveConstants.SaveCreatedKey, false))
            _resetWarningPanel.SetActive(true);
        else
            _gameCreationPanel.SetActive(true);

        gameObject.SetActive(false);
    }

    private void OnQuitGameButtonClicked() => Application.Quit();
}
