using Cysharp.Threading.Tasks;

public sealed class SceneSwitchAchievedViewMediator : SceneSwitchButtonViewMediator
{
    public SceneSwitchAchievedViewMediator(SceneSwitch sceneSwitch,
        AchievedLevelButtonView achievedLevelButtonView)
        : base(sceneSwitch, achievedLevelButtonView)
    {
    }

    protected override void OnButtonClicked() => SceneSwitch.LoadAchievedLevelAsync().Forget();
}
