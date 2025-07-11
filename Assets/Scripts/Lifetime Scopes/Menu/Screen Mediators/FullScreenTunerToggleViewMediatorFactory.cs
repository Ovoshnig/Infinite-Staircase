public class FullScreenTunerToggleViewMediatorFactory
    : MediatorViewFactory<FullScreenTunerToggleViewMediator, FullScreenToggleView>
{
    private readonly FullScreenTuner _fullScreenTuner;

    public FullScreenTunerToggleViewMediatorFactory(FullScreenTuner fullScreenTuner) => 
        _fullScreenTuner = fullScreenTuner;

    public override FullScreenTunerToggleViewMediator Create(FullScreenToggleView view)
    {
        FullScreenTunerToggleViewMediator mediator = new(_fullScreenTuner, view);
        mediator.Initialize();
        Disposables.Add(mediator);
        return mediator;
    }
}
