using Cysharp.Threading.Tasks;
using R3;

public class MusicPlayerSceneSwitchMediator : Mediator
{
    private readonly MusicPlayer _musicPlayer;
    private readonly SceneSwitch _sceneSwitch;

    public MusicPlayerSceneSwitchMediator(MusicPlayer musicPlayer, SceneSwitch sceneSwitch)
    {
        _musicPlayer = musicPlayer;
        _sceneSwitch = sceneSwitch;
    }

    public override void Initialize()
    {
        _sceneSwitch.IsSceneLoading
            .Where(loading => !loading)
            .Subscribe(_ => _musicPlayer.StartPlayMusicAsync(_sceneSwitch.CurrentSceneType).Forget())
            .AddTo(CompositeDisposable);
    }
}
