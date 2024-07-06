using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenu : Menu
{
    [SerializeField] private Button _continueGameButton;
    [SerializeField] private Button _startNewGameButton;
    [SerializeField] private Button _quitGameButton;
    [SerializeField] private Button _resetProgressButton;

    protected override void InitializeSettings()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    protected override void AddButtonListeners()
    {
        base.AddButtonListeners();

        _continueGameButton.onClick.AddListener(ContinueGame);
        _startNewGameButton.onClick.AddListener(StartNewGame);
        _quitGameButton.onClick.AddListener(QuitGame);
        _resetProgressButton.onClick.AddListener(ResetProgress);
    }

    protected override void RemoveButtonListeners()
    {
        base.RemoveButtonListeners();

        _continueGameButton.onClick.RemoveListener(ContinueGame);
        _startNewGameButton.onClick.RemoveListener(StartNewGame);
        _quitGameButton.onClick.RemoveListener(QuitGame);
        _resetProgressButton.onClick.RemoveListener(ResetProgress);
    }

    private void ContinueGame() => SceneSwitch.LoadAchievedLevel().Forget();
    private void StartNewGame() => SceneSwitch.LoadFirstLevel().Forget();
    private void QuitGame() => Application.Quit();
    private void ResetProgress() => SceneSwitch.ResetProgress();
}
