using R3;

public class VSyncToggleViewQualityTunerMediator : Mediator
{
    private readonly VSyncToggleView _vSyncToggleView;
    private readonly QualityTuner _qualityTuner;

    public VSyncToggleViewQualityTunerMediator(VSyncToggleView vSyncToggleView,
        QualityTuner qualityTuner)
    {
        _vSyncToggleView = vSyncToggleView;
        _qualityTuner = qualityTuner;
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
