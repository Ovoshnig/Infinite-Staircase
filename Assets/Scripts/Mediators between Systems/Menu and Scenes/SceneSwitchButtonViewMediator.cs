using R3;

public abstract class SceneSwitchButtonViewMediator : Mediator
{
    private readonly SceneSwitch _sceneSwitch;
    private readonly SceneButtonView _sceneButtonView;

    public SceneSwitchButtonViewMediator(SceneSwitch sceneSwitch, 
        SceneButtonView sceneButtonView)
    {
        _sceneSwitch = sceneSwitch;
        _sceneButtonView = sceneButtonView;
    }

    protected SceneSwitch SceneSwitch => _sceneSwitch;

    public override void Initialize()
    {
        _sceneButtonView.Clicked
            .Subscribe(_ => OnButtonClicked())
            .AddTo(CompositeDisposable);
    }

    protected abstract void OnButtonClicked();
}
