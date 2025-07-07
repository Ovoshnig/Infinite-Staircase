using Cysharp.Threading.Tasks;

public sealed class AchievedLevelButtonViewSceneSwitchMediator : SceneButtonViewSceneSwitchMediator
{
    public AchievedLevelButtonViewSceneSwitchMediator(AchievedLevelButtonView achievedLevelButtonView, 
        SceneSwitch sceneSwitch) 
        : base(achievedLevelButtonView, sceneSwitch)
    {
    }

    protected override void OnButtonClicked() => SceneSwitch.LoadAchievedLevelAsync().Forget();
}
