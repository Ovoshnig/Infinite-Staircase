using Cysharp.Threading.Tasks;
using R3;

public class SceneSwitchMusicPlayerMediator : Mediator
{
    private readonly SceneSwitch _sceneSwitch;
    private readonly MusicPlayer _musicPlayer;

    public SceneSwitchMusicPlayerMediator(SceneSwitch sceneSwitch, MusicPlayer musicPlayer)
    {
        _sceneSwitch = sceneSwitch;
        _musicPlayer = musicPlayer;
    }

    public override void Initialize()
    {
        _sceneSwitch.IsSceneLoading
            .Where(loading => !loading)
            .Subscribe(_ => _musicPlayer.LoadClipKeysAsync().Forget())
            .AddTo(CompositeDisposable);
    }
}
