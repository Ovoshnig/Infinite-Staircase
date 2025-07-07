using R3;

public abstract class SceneButtonViewSceneSwitchMediator : Mediator
{
    private readonly SceneButtonView _sceneButtonView;
    private readonly SceneSwitch _sceneSwitch;

    public SceneButtonViewSceneSwitchMediator(SceneButtonView sceneButtonView, 
        SceneSwitch sceneSwitch)
    {
        _sceneButtonView = sceneButtonView;
        _sceneSwitch = sceneSwitch;
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
