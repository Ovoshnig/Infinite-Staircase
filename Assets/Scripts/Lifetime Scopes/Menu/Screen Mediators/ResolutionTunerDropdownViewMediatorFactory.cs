public class ResolutionTunerDropdownViewMediatorFactory
    : MediatorViewFactory<ResolutionTunerDropdownViewMediator, ResolutionDropdownView>
{
    private readonly ResolutionTuner _resolutionTuner;

    public ResolutionTunerDropdownViewMediatorFactory(ResolutionTuner resolutionTuner) => 
        _resolutionTuner = resolutionTuner;

    public override ResolutionTunerDropdownViewMediator Create(ResolutionDropdownView view)
    {
        ResolutionTunerDropdownViewMediator mediator = new(_resolutionTuner, view);
        mediator.Initialize();
        Disposables.Add(mediator);
        return mediator;
    }
}
