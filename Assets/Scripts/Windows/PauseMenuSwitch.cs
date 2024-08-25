using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenuSwitch : Window
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Button _resumeButton;

    private GamePauser _gamePauser;

    [Inject]
    private void Construct(GamePauser gamePauser) => _gamePauser = gamePauser;

    protected override void InitializeInput() => 
        PlayerInput.PauseMenu.Switch.performed += OnSwitchPerformed;

    protected override void Awake()
    {
        base.Awake();
        _resumeButton.onClick.AddListener(OnResumeClicked);
    }

    private void OnDestroy() => 
        _resumeButton.onClick.RemoveListener(OnResumeClicked);

    public override void Open()
    {
        base.Open();
        _gamePauser.Pause();
    }

    public override void Close()
    {
        if (_settingsPanel.activeSelf)
        {
            Panel.SetActive(true);
            _settingsPanel.SetActive(false);
            return;
        }

        base.Close();
        _gamePauser.Unpause();
    }

    private void OnResumeClicked() => Close();
}