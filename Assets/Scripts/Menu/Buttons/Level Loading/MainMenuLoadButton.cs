using Cysharp.Threading.Tasks;

public sealed class MainMenuLoadButton : LevelLoadButton
{
    protected override void OnButtonClicked() => SceneSwitch.LoadLevelAsync(0).Forget();
}
