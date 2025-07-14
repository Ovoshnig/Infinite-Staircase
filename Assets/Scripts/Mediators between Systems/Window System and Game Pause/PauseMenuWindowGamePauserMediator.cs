using R3;

public class PauseMenuWindowGamePauserMediator : Mediator
{
    private readonly PauseMenuWindow _pauseMenuWindow;
    private readonly GamePauser _gamePauser;

    public PauseMenuWindowGamePauserMediator(PauseMenuWindow pauseMenuWindow, GamePauser gamePauser)
    {
        _pauseMenuWindow = pauseMenuWindow;
        _gamePauser = gamePauser;
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
