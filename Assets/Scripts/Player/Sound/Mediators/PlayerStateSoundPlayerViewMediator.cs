using R3;

public class PlayerStateSoundPlayerViewMediator : Mediator
{
    private readonly PlayerState _playerState;
    private readonly PlayerSoundPlayerView _playerSoundPlayerView;

    public PlayerStateSoundPlayerViewMediator(PlayerState playerState, 
        PlayerSoundPlayerView playerSoundPlayerView)
    {
        _playerState = playerState;
        _playerSoundPlayerView = playerSoundPlayerView;
    }

    public override void Initialize()
    {
        _playerState.Grounded
                .Where(isGrounded => isGrounded)
                .Subscribe(_ => _playerSoundPlayerView.PlayLandSound())
                .AddTo(CompositeDisposable);
    }
}
