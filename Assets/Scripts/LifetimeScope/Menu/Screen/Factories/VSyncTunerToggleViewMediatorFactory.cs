public class VSyncTunerToggleViewMediatorFactory
    : MediatorViewFactory<VSyncTunerToggleViewMediator, VSyncToggleView>
{
    private readonly VSyncTuner _vSyncTuner;

    public VSyncTunerToggleViewMediatorFactory(VSyncTuner vSyncTuner) =>
        _vSyncTuner = vSyncTuner;

    protected override VSyncTunerToggleViewMediator CreateMediatorInstance(VSyncToggleView view) =>
        new(_vSyncTuner, view);
}
