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
    private CompositeDisposable _compositeDisposable = new();

    [Inject]
    public void Construct(SaveStorage saveStorage, SceneSwitch sceneSwitch)
    {
        _saveStorage = saveStorage;
        _sceneSwitch = sceneSwitch;
    }

    private void OnEnable()
    {
        _continueGameButton.OnClickAsObservable()
            .Subscribe(_ => OnContinueGameButtonClicked())
            .AddTo(_compositeDisposable);
        _newGameButton.OnClickAsObservable()
            .Subscribe(_ => OnNewGameButtonClicked())
            .AddTo(_compositeDisposable);
        _quitGameButton.OnClickAsObservable()
            .Subscribe(_ => OnQuitGameButtonClicked())
            .AddTo(_compositeDisposable);
    }

    private void Start()
    {
        bool saveCreated = _saveStorage.Get(SaveConstants.SaveCreatedKey, false);
        _continueGameButton.interactable = saveCreated;
    }

    private void OnDisable()
    {
        _compositeDisposable?.Dispose();
        _compositeDisposable = new CompositeDisposable();
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
