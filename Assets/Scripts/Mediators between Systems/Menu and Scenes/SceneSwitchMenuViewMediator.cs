using Cysharp.Threading.Tasks;

public sealed class SceneSwitchMenuViewMediator : SceneSwitchButtonViewMediator
{
    public SceneSwitchMenuViewMediator(SceneSwitch sceneSwitch,
        MainMenuButtonView mainMenuButtonView)
        : base(sceneSwitch, mainMenuButtonView)
    {
    }

    protected override void OnButtonClicked() => SceneSwitch.LoadLevelAsync(0).Forget();
}
