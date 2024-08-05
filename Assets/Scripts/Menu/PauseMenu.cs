using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
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

    private WindowTracker _windowTracker;
    private PlayerInput _playerInput;
    private bool _paused = false;

    public event Action Paused;
    public event Action Resumed;

    [Inject]
    private void Construct(WindowTracker windowTracker) => _windowTracker = windowTracker;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.PauseMenu.OpenOrClose.performed += OnOpenOrClose;
    }

    protected override void InitializeSettings() => Resume();

    protected override void SubscribeToEvents()
    {
        base.SubscribeToEvents();

        _playerInput.Enable();
    }

    protected override void UnsubscribeFromEvents()
    {
        base.UnsubscribeFromEvents();

        _playerInput.Disable();
    }

    protected override void AddButtonListeners()
    {
        base.AddButtonListeners();

        _resumeButton.onClick.AddListener(OnResumeClicked);
        _resetLevelButton.onClick.AddListener(ResetLevel);
        _loadNextLevelButton.onClick.AddListener(LoadNextLevel);
        _loadPreviousLevelButton.onClick.AddListener(LoadPreviousLevel);
        _loadMainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    protected override void RemoveButtonListeners()
    {
        base.RemoveButtonListeners();

        _resumeButton.onClick.RemoveListener(OnResumeClicked);
        _resetLevelButton.onClick.RemoveListener(ResetLevel);
        _loadNextLevelButton.onClick.RemoveListener(LoadNextLevel);
        _loadPreviousLevelButton.onClick.RemoveListener(LoadPreviousLevel);
        _loadMainMenuButton.onClick.RemoveListener(LoadMainMenu);
    }

    private void ResetLevel() => SceneSwitch.LoadCurrentLevel();

    private void LoadPreviousLevel() => SceneSwitch.LoadPreviousLevel().Forget();

    private void LoadNextLevel() => SceneSwitch.LoadNextLevel().Forget();

    private void LoadMainMenu() => SceneSwitch.LoadLevel(0).Forget();

    private void OnOpenOrClose(InputAction.CallbackContext _)
    {
        _paused = !_paused;

        if (_paused)
            Pause();
        else
            Resume();
    }

    private void OnResumeClicked()
    {
        _paused = false;
        Resume();
    }

    private void Pause()
    {
        if (!_windowTracker.TryOpenWindow(MenuPanel))
        {
            _paused = false;
            return;
        }

        MenuPanel.SetActive(true);
        _playerPoint.SetActive(false);
        Paused?.Invoke();
    }

    private void Resume()
    {
        _windowTracker.CloseWindow();

        MenuPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        _playerPoint.SetActive(true);
        Resumed?.Invoke();
    }
}
