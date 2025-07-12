using R3;

public class FullScreenTunerToggleViewMediator : Mediator
{
    private readonly FullScreenTuner _fullScreenTuner;
    private readonly FullScreenToggleView _fullScreenToggleView;

    public FullScreenTunerToggleViewMediator(FullScreenTuner fullScreenTuner,
        FullScreenToggleView fullScreenToggleView)
    {
        _fullScreenTuner = fullScreenTuner;
        _fullScreenToggleView = fullScreenToggleView;
    }

    public override void Initialize()
    {
        _fullScreenTuner.IsFullScreen
            .Subscribe(_fullScreenToggleView.SetIsOnWithoutNotify)
            .AddTo(CompositeDisposable);

        _fullScreenToggleView.IsOn
            .Subscribe(isOn =>
            {
                if (isOn)
                    _fullScreenTuner.EnableFullScreen();
                else
                    _fullScreenTuner.DisableFullScreen();
            })
            .AddTo(CompositeDisposable);
    }
}
