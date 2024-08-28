using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public sealed class MainMenu : Menu
{
    [SerializeField] private Button _continueGameButton;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _quitGameButton;
    [SerializeField] private GameObject _resetWarningPanel;
    [SerializeField] private GameObject _gameCreationPanel;

    private SaveStorage _saveStorage;

    [Inject]
    private void Construct(SaveStorage saveStorage) => _saveStorage = saveStorage;

    protected override void OnEnable()
    {
        base.OnEnable();

        _continueGameButton.interactable = _saveStorage.Get(SaveConstants.SaveCreatedKey, false);
    }

    protected override void InitializeSettings()
    {
        base.InitializeSettings();

        gameObject.SetActive(true);
        _resetWarningPanel.SetActive(false);
    }

    protected override void AddListeners()
    {
        base.AddListeners();

        _continueGameButton.onClick.AddListener(OnContinueGameButtonClicked);
        _newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        _quitGameButton.onClick.AddListener(OnQuitGameButtonClicked);
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();

        _continueGameButton.onClick.RemoveListener(OnContinueGameButtonClicked);
        _newGameButton.onClick.RemoveListener(OnNewGameButtonClicked);
        _quitGameButton.onClick.RemoveListener(OnQuitGameButtonClicked);
    }

    private void OnContinueGameButtonClicked() => SceneSwitch.LoadAchievedLevel().Forget();

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
