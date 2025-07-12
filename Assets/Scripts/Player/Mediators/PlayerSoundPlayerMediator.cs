using R3;

public class PlayerSoundPlayerMediator : Mediator
{
    private readonly PlayerSoundPlayer _playerSoundPlayer;
    private readonly PlayerSoundPlayerView _playerSoundPlayerView;

    public PlayerSoundPlayerMediator(PlayerSoundPlayer playerSoundPlayer, 
        PlayerSoundPlayerView playerSoundPlayerView)
    {
        _playerSoundPlayer = playerSoundPlayer;
        _playerSoundPlayerView = playerSoundPlayerView;
    }

    public override void Initialize()
    {
        _playerSoundPlayer.SetReferences(
            _playerSoundPlayerView.FootstepReference, 
            _playerSoundPlayerView.LandReference);

        _playerSoundPlayer.ResourcesLoaded
            .Subscribe(resources => _playerSoundPlayerView.SetResources(resources.Item1, resources.Item2))
            .AddTo(CompositeDisposable);
    }
}
