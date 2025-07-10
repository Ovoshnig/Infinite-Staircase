using Cysharp.Threading.Tasks;

public sealed class SceneSwitchCurrentViewMediator : SceneSwitchButtonViewMediator
{
    public SceneSwitchCurrentViewMediator(SceneSwitch sceneSwitch,
        CurrentLevelButtonView currentLevelButtonView)
        : base(sceneSwitch, currentLevelButtonView)
    {
    }

    protected override void OnButtonClicked() => SceneSwitch.LoadCurrentLevelAsync().Forget();
}
