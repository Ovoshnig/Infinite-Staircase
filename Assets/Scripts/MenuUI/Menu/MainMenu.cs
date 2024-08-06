using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenu : Menu
{
    [SerializeField] private Button _continueGameButton;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _quitGameButton;
    [SerializeField] private GameObject _resetWarningPanel;

    protected override void InitializeSettings()
    {
        SettingsPanel.SetActive(false);
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
        _resetWarningPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnQuitGameButtonClicked() => Application.Quit();
}
