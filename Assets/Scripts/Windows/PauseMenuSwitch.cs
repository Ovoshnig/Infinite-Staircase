using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class PauseMenuSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Button _resumeButton;

    private PlayerInput _playerInput;
    private WindowTracker _windowTracker;
    private GamePauser _gamePauser;
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
        _playerInput.PauseMenu.Switch.performed += OnSwitchPerformed;
        _playerInput.Enable();

        _resumeButton.onClick.AddListener(OnResumeClicked);
    }

    private void OnDisable()
    {
        _playerInput.Disable();

        _resumeButton.onClick.RemoveListener(OnResumeClicked);
    }

    private void OnSwitchPerformed(InputAction.CallbackContext _)
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
        _menuPanel.SetActive(true);
        _gamePauser.Pause();
        Opened?.Invoke();
    }

    private void Close()
    {
        if (_settingsPanel.activeSelf)
        {
            _menuPanel.SetActive(true);
            _settingsPanel.SetActive(false);

            return;
        }

        if (!_windowTracker.TryCloseWindow())
            return;

        _opened = false;
        _menuPanel.SetActive(false);
        _gamePauser.Unpause();
        Closed?.Invoke();
    }
}
