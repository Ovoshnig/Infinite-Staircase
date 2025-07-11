public class SceneSwitchButtonViewMediatorFactory 
    : MediatorViewFactory<SceneSwitchButtonViewMediator, SceneButtonView>
{
    private readonly SceneSwitch _sceneSwitch;

    public SceneSwitchButtonViewMediatorFactory(SceneSwitch sceneSwitch) => 
        _sceneSwitch = sceneSwitch;

    public override SceneSwitchButtonViewMediator Create(SceneButtonView view)
    {
        SceneSwitchButtonViewMediator mediator = new(_sceneSwitch, view);
        mediator.Initialize();
        Disposables.Add(mediator);
        return mediator;
    }
}
