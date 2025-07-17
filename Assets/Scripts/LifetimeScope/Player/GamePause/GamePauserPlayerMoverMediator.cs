using R3;

public class GamePauserPlayerMoverMediator : Mediator
{
    private readonly GamePauser _gamePauser;
    private readonly PlayerMover _playerMover;

    public GamePauserPlayerMoverMediator(GamePauser gamePauser, PlayerMover playerMover)
    {
        _gamePauser = gamePauser;
        _playerMover = playerMover;
    }

    public override void Initialize()
    {
        _gamePauser.IsPause
            .Subscribe(_playerMover.SetPause)
            .AddTo(CompositeDisposable);
    }
}
