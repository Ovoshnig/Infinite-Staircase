using R3;

public class QualityTunerVSyncToggleViewMediator : Mediator
{
    private readonly QualityTuner _qualityTuner;
    private readonly VSyncToggleView _vSyncToggleView;

    public QualityTunerVSyncToggleViewMediator(QualityTuner qualityTuner,
        VSyncToggleView vSyncToggleView)
    {
        _qualityTuner = qualityTuner;
        _vSyncToggleView = vSyncToggleView;
    }

    public override void Initialize()
    {
        _vSyncToggleView.IsOn
            .Skip(1)
            .Subscribe(value =>
            {
                if (value)
                    _qualityTuner.EnableVSync();
                else
                    _qualityTuner.DisableVSync();
            })
            .AddTo(CompositeDisposable);

        _qualityTuner.IsVSyncEnabled
            .Subscribe(value =>
            {
                if (value)
                    _vSyncToggleView.Enable();
                else
                    _vSyncToggleView.Disable();
            })
            .AddTo(CompositeDisposable);
    }
}
