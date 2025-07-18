public class SceneSwitchButtonViewMediatorFactory
    : MediatorViewFactory<SceneSwitchButtonViewMediator, SceneButtonView>
{
    private readonly SceneSwitch _sceneSwitch;

    public SceneSwitchButtonViewMediatorFactory(SceneSwitch sceneSwitch) =>
        _sceneSwitch = sceneSwitch;

    protected override SceneSwitchButtonViewMediator CreateMediatorInstance(SceneButtonView view) =>
        new(_sceneSwitch, view);
}
