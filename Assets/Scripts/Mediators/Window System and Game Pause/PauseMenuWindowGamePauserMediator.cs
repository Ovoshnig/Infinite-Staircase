using R3;
using System;
using VContainer.Unity;

public class PauseMenuWindowGamePauserMediator : IInitializable, IDisposable
{
    private readonly PauseMenuWindow _pauseMenuWindow;
    private readonly GamePauser _gamePauser;
    private readonly CompositeDisposable _compositeDisposable = new();

    public PauseMenuWindowGamePauserMediator(PauseMenuWindow pauseMenuWindow, GamePauser gamePauser)
    {
        _pauseMenuWindow = pauseMenuWindow;
        _gamePauser = gamePauser;
    }

    public void Initialize()
    {
        _pauseMenuWindow.IsOpen
            .Subscribe(value =>
            {
                if (value)
                    _gamePauser.Pause();
                else
                    _gamePauser.Unpause();
            })
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
