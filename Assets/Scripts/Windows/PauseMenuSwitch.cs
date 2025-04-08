using R3;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuSwitch : WindowSwitch
{
    [SerializeField] private Button _resumeButton;

    private readonly ReactiveProperty<bool> _pauseMenuOpened = new(false);

    public ReadOnlyReactiveProperty<bool> PauseMenuOpened => _pauseMenuOpened;

    protected override void InitializeInput()
    {
        Disposable = WindowInputHandler.PauseMenuSwitchPressed
            .Where(value => value)
            .Subscribe(_ => Switch());
    }

    protected override void Start()
    {
        base.Start();

        _resumeButton.OnClickAsObservable()
            .Subscribe(_ => OnResumeClicked())
            .AddTo(this);
    }

    public override bool Open()
    {
        if (!base.Open())
            return false;
        
        _pauseMenuOpened.Value = true;

        return true;
    }

    public override bool Close()
    {
        if (!Panel.activeSelf || !base.Close())
            return false;

        _pauseMenuOpened.Value = false;

        return true;
    }

    private void OnResumeClicked() => Close();
}
