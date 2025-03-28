using Cysharp.Threading.Tasks;

public sealed class AchievedLevelLoadButton : LevelLoadButton
{
    protected override void OnButtonClicked() => SceneSwitch.LoadAchievedLevelAsync().Forget();
}
