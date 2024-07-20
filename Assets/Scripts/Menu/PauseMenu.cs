using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public sealed class PauseMenu : Menu
{
    [SerializeField] private GameObject _playerPoint;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _resetLevelButton;
    [SerializeField] private Button _loadNextLevelButton;
    [SerializeField] private Button _loadPreviousLevelButton;
    [SerializeField] private Button _loadMainMenuButton;

    private GamePauser _gamePauser;

    public event Action OnResumeClicked;
        
    protected override void InitializeSettings() => Resume();

    protected override void SubscribeToEvents()
    {
        base.SubscribeToEvents();

        _gamePauser.GamePaused += Pause;
        _gamePauser.GameUnpaused += Resume;
    }

    protected override void UnsubscribeFromEvents()
    {
        base.UnsubscribeFromEvents();

        _gamePauser.GamePaused -= Pause;
        _gamePauser.GameUnpaused -= Resume;
    }

    protected override void AddButtonListeners()
    {
        base.AddButtonListeners();

        _resumeButton.onClick.AddListener(() => OnResumeClicked?.Invoke());
        _resetLevelButton.onClick.AddListener(ResetLevel);
        _loadNextLevelButton.onClick.AddListener(LoadNextLevel);
        _loadPreviousLevelButton.onClick.AddListener(LoadPreviousLevel);
        _loadMainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    protected override void RemoveButtonListeners()
    {
        base.RemoveButtonListeners();

        _resumeButton.onClick.RemoveListener(() => OnResumeClicked?.Invoke());
        _resetLevelButton.onClick.RemoveListener(ResetLevel);
        _loadNextLevelButton.onClick.RemoveListener(LoadNextLevel);
        _loadPreviousLevelButton.onClick.RemoveListener(LoadPreviousLevel);
        _loadMainMenuButton.onClick.RemoveListener(LoadMainMenu);
    }

    [Inject]
    private void Construct(GamePauser gamePauser) => _gamePauser = gamePauser;
    private void ResetLevel() => SceneSwitch.LoadCurrentLevel();
    private void LoadPreviousLevel() => SceneSwitch.LoadPreviousLevel().Forget();
    private void LoadNextLevel() => SceneSwitch.LoadNextLevel().Forget();
    private void LoadMainMenu() => SceneSwitch.LoadLevel(0).Forget();

    private void Pause()
    {
        MenuPanel.SetActive(true);
        _playerPoint.SetActive(false);
    }

    private void Resume()
    {
        MenuPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        _playerPoint.SetActive(true);
    }

    public class Factory : PlaceholderFactory<PauseMenu> { }
}
