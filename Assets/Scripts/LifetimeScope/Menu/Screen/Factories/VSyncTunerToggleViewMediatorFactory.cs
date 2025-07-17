public class VSyncTunerToggleViewMediatorFactory
    : MediatorViewFactory<VSyncTunerToggleViewMediator, VSyncToggleView>
{
    private readonly VSyncTuner _vSyncTuner;

    public VSyncTunerToggleViewMediatorFactory(VSyncTuner vSyncTuner) => 
        _vSyncTuner = vSyncTuner;

    public override VSyncTunerToggleViewMediator Create(VSyncToggleView view)
    {
        VSyncTunerToggleViewMediator mediator = new(_vSyncTuner, view);
        mediator.Initialize();
        Disposables.Add(mediator);
        return mediator;
    }
}
