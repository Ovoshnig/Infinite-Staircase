using Cysharp.Threading.Tasks;

public sealed class MainMenuButtonViewSceneSwitchMediator : SceneButtonViewSceneSwitchMediator
{
    public MainMenuButtonViewSceneSwitchMediator(MainMenuButtonView mainMenuButtonView, 
        SceneSwitch sceneSwitch)
        : base(mainMenuButtonView, sceneSwitch)
    {
    }

    protected override void OnButtonClicked() => SceneSwitch.LoadLevelAsync(0).Forget();
}
