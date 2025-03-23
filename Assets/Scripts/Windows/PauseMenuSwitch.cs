using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class PauseMenuSwitch : WindowSwitch
{
    [SerializeField] private Button _resumeButton;

    private GamePauser _gamePauser;
    private CompositeDisposable _compositeDisposable = new();

    [Inject]
    public void Construct(GamePauser gamePauser) => _gamePauser = gamePauser;

    protected override void InitializeInput()
    {
        Disposable = WindowInputHandler.PauseMenuSwitchPressed
            .Where(value => value)
            .Subscribe(_ => Switch());
    }

    protected override void Awake()
    {
        base.Awake();

        _resumeButton.OnClickAsObservable()
            .Subscribe(_ => OnResumeClicked())
            .AddTo(_compositeDisposable);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _compositeDisposable?.Dispose();
    }

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
