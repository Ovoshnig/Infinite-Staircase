using R3;
using System;
using VContainer.Unity;

public class VSyncToggleViewQualityTunerMediator : IInitializable, IDisposable
{
    private readonly VSyncToggleView _vSyncToggleView;
    private readonly QualityTuner _qualityTuner;
    private readonly CompositeDisposable _compositeDisposable = new();

    public VSyncToggleViewQualityTunerMediator(VSyncToggleView vSyncToggleView,
        QualityTuner qualityTuner)
    {
        _vSyncToggleView = vSyncToggleView;
        _qualityTuner = qualityTuner;
    }

    public void Initialize()
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
            .AddTo(_compositeDisposable);
        _qualityTuner.IsVSyncEnabled
            .Subscribe(value =>
            {
                if (value)
                    _vSyncToggleView.Enable();
                else
                    _vSyncToggleView.Disable();
            })
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
