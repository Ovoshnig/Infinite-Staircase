using R3;

public class PlayerMoverGamePauserMediator : Mediator
{
    private readonly PlayerMover _playerMover;
    private readonly GamePauser _gamePauser;

    public PlayerMoverGamePauserMediator(PlayerMover playerMover, GamePauser gamePauser)
    {
        _playerMover = playerMover;
        _gamePauser = gamePauser;
    }

    public override void Initialize()
    {
        _gamePauser.IsPause
            .Subscribe(_playerMover.SetPause)
            .AddTo(CompositeDisposable);
    }
}
