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
    private GamePauser _gamePauser;
    private PlayerInput _playerInput;
    private bool _opened = false;

    public event Action Opened;
    public event Action Closed;

    [Inject]
    private void Construct(WindowTracker windowTracker, GamePauser gamePauser)
    {
        _windowTracker = windowTracker;
        _gamePauser = gamePauser;
    }

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.PauseMenu.OpenOrClose.performed += OnOpenOrClosePerformed;
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

    private void OnOpenOrClosePerformed(InputAction.CallbackContext _)
    {
        if (_opened)
            Close();
        else
            Open();
    }

    private void OnResumeClicked() => Close();

    private void Open()
    {
        if (!_windowTracker.TryOpenWindow(gameObject))
            return;

        _opened = true;
        gameObject.SetActive(true);
        _gamePauser.Pause();
        Opened?.Invoke();
    }

    private void Close()
    {
        _windowTracker.TryCloseWindow();

        _opened = false;
        gameObject.SetActive(false);
        SettingsPanel.SetActive(false);
        _gamePauser.Unpause();
        Closed?.Invoke();
    }
}
