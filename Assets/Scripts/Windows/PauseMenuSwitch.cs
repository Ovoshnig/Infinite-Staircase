using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenuSwitch : WindowSwitch
{
    [SerializeField] private Button _resumeButton;

    private GamePauser _gamePauser;

    [Inject]
    private void Construct(GamePauser gamePauser) => _gamePauser = gamePauser;

    protected override void InitializeInput()
    {
        InputHandler.PauseMenuSwitchPressed
            .Where(value => value)
            .Subscribe(_ => Switch());
    }

    protected override void Awake()
    {
        base.Awake();
        _resumeButton.onClick.AddListener(OnResumeClicked);
    }

    private void OnDestroy() => 
        _resumeButton.onClick.RemoveListener(OnResumeClicked);

    public override bool Open()
    {
        if (!base.Open())
            return false;
        
        _gamePauser.Pause();

        return true;
    }

    public override bool Close()
    {
        if (!Panel.activeSelf || !base.Close())
            return false;

        _gamePauser.Unpause();

        return true;
    }

    private void OnResumeClicked() => Close();
}
