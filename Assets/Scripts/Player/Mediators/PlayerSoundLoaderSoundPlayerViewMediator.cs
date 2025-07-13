public class PlayerSoundLoaderSoundPlayerViewMediator : Mediator
{
    private readonly PlayerSoundLoader _playerSoundLoader;
    private readonly PlayerSoundPlayerView _playerSoundPlayerView;

    public PlayerSoundLoaderSoundPlayerViewMediator(PlayerSoundLoader playerSoundLoader, 
        PlayerSoundPlayerView playerSoundPlayerView)
    {
        _playerSoundLoader = playerSoundLoader;
        _playerSoundPlayerView = playerSoundPlayerView;
    }

    public async override void Initialize()
    {
        var (footstepResource, landResource) = await _playerSoundLoader.LoadSoundsAsync(
            _playerSoundPlayerView.FootstepReference, 
            _playerSoundPlayerView.LandReference);
        _playerSoundPlayerView.SetResources(footstepResource, landResource);
    }

    public override void Dispose()
    {
        base.Dispose();

        _playerSoundLoader.ReleaseSounds();
    }
}
