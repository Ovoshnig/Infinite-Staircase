using R3;

public class ScreenTunerFullScreenToggleViewMediator : Mediator
{
    private readonly ScreenTuner _screenTuner;
    private readonly FullScreenToggleView _fullScreenToggleView;

    public ScreenTunerFullScreenToggleViewMediator(ScreenTuner screenTuner,
        FullScreenToggleView fullScreenToggleView)
    {
        _screenTuner = screenTuner;
        _fullScreenToggleView = fullScreenToggleView;
    }

    public override void Initialize()
    {
        _fullScreenToggleView.IsOn
            .Skip(1)
            .Subscribe(value =>
            {
                if (value)
                    _screenTuner.EnableFullScreen();
                else
                    _screenTuner.DisableFullScreen();
            })
            .AddTo(CompositeDisposable);
        _screenTuner.IsFullScreen
            .Subscribe(value =>
            {
                if (value)
                    _fullScreenToggleView.Enable();
                else
                    _fullScreenToggleView.Disable();
            })
            .AddTo(CompositeDisposable);
    }
}
