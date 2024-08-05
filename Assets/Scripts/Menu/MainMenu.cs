using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenu : Menu
{
    [SerializeField] private GameObject _gameCreationPanel;
    [SerializeField] private Button _continueGameButton;
    [SerializeField] private Button _openCreateGamePanelButton;
    [SerializeField] private Button _quitGameButton;

    protected override void InitializeSettings()
    {
        SettingsPanel.SetActive(false);
        _gameCreationPanel.SetActive(false);
    }

    protected override void AddButtonListeners()
    {
        base.AddButtonListeners();

        _continueGameButton.onClick.AddListener(ContinueGame);
        _openCreateGamePanelButton.onClick.AddListener(OpenGameCreationPanel);
        _quitGameButton.onClick.AddListener(QuitGame);
    }

    protected override void RemoveButtonListeners()
    {
        base.RemoveButtonListeners();

        _continueGameButton.onClick.RemoveListener(ContinueGame);
        _openCreateGamePanelButton.onClick.RemoveListener(OpenGameCreationPanel);
        _quitGameButton.onClick.RemoveListener(QuitGame);
    }

    private void ContinueGame() => SceneSwitch.LoadAchievedLevel().Forget();

    private void OpenGameCreationPanel() => _gameCreationPanel.SetActive(true);

    private void QuitGame() => Application.Quit();
}
