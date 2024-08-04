using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenu : Menu
{
    [SerializeField] private Button _continueGameButton;
    [SerializeField] private Button _startNewGameButton;
    [SerializeField] private Button _quitGameButton;

    protected override void InitializeSettings()
    {
        SettingsPanel.SetActive(false);

        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
    }

    protected override void AddButtonListeners()
    {
        base.AddButtonListeners();

        _continueGameButton.onClick.AddListener(ContinueGame);
        _startNewGameButton.onClick.AddListener(StartNewGame);
        _quitGameButton.onClick.AddListener(QuitGame);
    }

    protected override void RemoveButtonListeners()
    {
        base.RemoveButtonListeners();

        _continueGameButton.onClick.RemoveListener(ContinueGame);
        _startNewGameButton.onClick.RemoveListener(StartNewGame);
        _quitGameButton.onClick.RemoveListener(QuitGame);
    }

    private void ContinueGame() => SceneSwitch.LoadAchievedLevel().Forget();

    private void StartNewGame() => SceneSwitch.LoadFirstLevel().Forget();

    private void QuitGame() => Application.Quit();
}
