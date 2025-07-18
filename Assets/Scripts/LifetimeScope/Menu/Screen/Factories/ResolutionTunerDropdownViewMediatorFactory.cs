public class ResolutionTunerDropdownViewMediatorFactory
    : MediatorViewFactory<ResolutionTunerDropdownViewMediator, ResolutionDropdownView>
{
    private readonly ResolutionTuner _resolutionTuner;

    public ResolutionTunerDropdownViewMediatorFactory(ResolutionTuner resolutionTuner) =>
        _resolutionTuner = resolutionTuner;

    protected override ResolutionTunerDropdownViewMediator CreateMediatorInstance(ResolutionDropdownView view) =>
        new(_resolutionTuner, view);
}
