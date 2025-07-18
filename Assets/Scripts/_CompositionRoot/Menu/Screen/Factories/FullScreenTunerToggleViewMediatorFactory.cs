public class FullScreenTunerToggleViewMediatorFactory
    : MediatorViewFactory<FullScreenTunerToggleViewMediator, FullScreenToggleView>
{
    private readonly FullScreenTuner _fullScreenTuner;

    public FullScreenTunerToggleViewMediatorFactory(FullScreenTuner fullScreenTuner) =>
        _fullScreenTuner = fullScreenTuner;

    protected override FullScreenTunerToggleViewMediator CreateMediatorInstance(FullScreenToggleView view) =>
        new(_fullScreenTuner, view);
}
