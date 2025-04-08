public sealed class CurrentLevelButtonViewSceneSwitchMediator : SceneButtonViewSceneSwitchMediator
{
    public CurrentLevelButtonViewSceneSwitchMediator(CurrentLevelButtonView currentLevelButtonView,
        SceneSwitch sceneSwitch) 
        : base(currentLevelButtonView, sceneSwitch)
    {
    }

    protected override void OnButtonClicked() => SceneSwitch.LoadCurrentLevel();
}
