using R3;

public class GamePauserPauseMenuWindowMediator : Mediator
{
    private readonly GamePauser _gamePauser;
    private readonly PauseMenuWindow _pauseMenuWindow;

    public GamePauserPauseMenuWindowMediator(GamePauser gamePauser, PauseMenuWindow pauseMenuWindow)
    {
        _gamePauser = gamePauser;
        _pauseMenuWindow = pauseMenuWindow;
    }

    public override void Initialize()
    {
        _pauseMenuWindow.IsOpen
            .Subscribe(isOpen =>
            {
                if (isOpen)
                    _gamePauser.Pause();
                else
                    _gamePauser.UnPause();
            })
            .AddTo(CompositeDisposable);
    }
}
