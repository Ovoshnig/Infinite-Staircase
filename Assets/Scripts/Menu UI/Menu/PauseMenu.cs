using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public sealed class PauseMenu : Menu
{
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
        _playerInput.Enable();
    }

    private void OnDestroy() => _playerInput.Disable();

    protected override void InitializeSettings()
    {
        gameObject.SetActive(false);
        SettingsPanel.SetActive(false);
    }

    protected override void AddListeners()
    {
        base.AddListeners();

        _resumeButton.onClick.AddListener(OnResumeClicked);
        _resetLevelButton.onClick.AddListener(OnResetButtonClicked);
        _loadPreviousLevelButton.onClick.AddListener(OnPreviousLevelButtonClicked);
        _loadNextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        _loadMainMenuButton.onClick.AddListener(OnLoadMainMenuButtonClicked);
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();

        _resumeButton.onClick.RemoveListener(OnResumeClicked);
        _resetLevelButton.onClick.RemoveListener(OnResetButtonClicked);
        _loadPreviousLevelButton.onClick.RemoveListener(OnPreviousLevelButtonClicked);
        _loadNextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);
        _loadMainMenuButton.onClick.RemoveListener(OnLoadMainMenuButtonClicked);
    }

    private void OnResetButtonClicked() => SceneSwitch.LoadCurrentLevel();

    private void OnPreviousLevelButtonClicked() => SceneSwitch.LoadPreviousLevel().Forget();

    private void OnNextLevelButtonClicked() => SceneSwitch.LoadNextLevel().Forget();

    private void OnLoadMainMenuButtonClicked() => SceneSwitch.LoadLevel(0).Forget();

    private void OnOpenOrClose(InputAction.CallbackContext _)
    {
        if (_paused)
            Resume();
        else
            Pause();
    }

    private void OnResumeClicked() => Resume();

    private void Pause()
    {
        if (!_windowTracker.TryOpenWindow(gameObject))
            return;

        _paused = true;
        Paused?.Invoke();
    }

    private void Resume()
    {
        if (!_windowTracker.TryCloseWindow())
            return;

        _paused = false;
        SettingsPanel.SetActive(false);
        Resumed?.Invoke();
    }
}