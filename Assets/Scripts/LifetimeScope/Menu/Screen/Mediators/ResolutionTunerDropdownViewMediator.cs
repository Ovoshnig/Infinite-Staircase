using R3;

public class ResolutionTunerDropdownViewMediator : Mediator
{
    private readonly ResolutionTuner _resolutionTuner;
    private readonly ResolutionDropdownView _resolutionDropdownView;

    public ResolutionTunerDropdownViewMediator(ResolutionTuner resolutionTuner,
        ResolutionDropdownView resolutionDropdownView)
    {
        _resolutionTuner = resolutionTuner;
        _resolutionDropdownView = resolutionDropdownView;
    }

    public override void Initialize()
    {
        _resolutionDropdownView.SetResolutionOptions(_resolutionTuner.Resolutions);
        _resolutionDropdownView.SetValueWithoutNotify(_resolutionTuner.CurrentResolutionNumber);

        _resolutionDropdownView.Value
            .Subscribe(_resolutionTuner.SetResolution)
            .AddTo(CompositeDisposable);
    }
}
