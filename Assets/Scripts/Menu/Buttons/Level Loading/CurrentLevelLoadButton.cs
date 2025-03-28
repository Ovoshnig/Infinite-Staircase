using Cysharp.Threading.Tasks;
using R3;
public sealed class CurrentLevelLoadButton : LevelLoadButton
{
    protected override void OnButtonClicked() => SceneSwitch.LoadCurrentLevel();
}
