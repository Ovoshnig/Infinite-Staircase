using R3;

public class FullScreenToggleViewScreenTunerMediator : Mediator
{
    private readonly FullScreenToggleView _fullScreenToggleView;
    private readonly ScreenTuner _screenTuner;

    public FullScreenToggleViewScreenTunerMediator(FullScreenToggleView fullScreenToggleView, 
        ScreenTuner screenTuner)
    {
        _fullScreenToggleView = fullScreenToggleView;
        _screenTuner = screenTuner;
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
