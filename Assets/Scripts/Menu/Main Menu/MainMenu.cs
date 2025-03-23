using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

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
    public void Construct(SaveStorage saveStorage, SceneSwitch sceneSwitch)
    {
        _saveStorage = saveStorage;
        _sceneSwitch = sceneSwitch;
    }

    private void Start()
    {
        _continueGameButton.OnClickAsObservable()
            .Subscribe(_ => OnContinueGameButtonClicked())
            .AddTo(this);
        _newGameButton.OnClickAsObservable()
            .Subscribe(_ => OnNewGameButtonClicked())
            .AddTo(this);
        _quitGameButton.OnClickAsObservable()
            .Subscribe(_ => OnQuitGameButtonClicked())
            .AddTo(this);

        bool saveCreated = _saveStorage.Get(SaveConstants.SaveCreatedKey, false);
        _continueGameButton.interactable = saveCreated;
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
