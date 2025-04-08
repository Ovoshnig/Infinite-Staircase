using R3;
using System;
using VContainer.Unity;

public class PauseMenuSwitchGamePauserMediator : IInitializable, IDisposable
{
    private readonly PauseMenuSwitch _pauseMenuSwitch;
    private readonly GamePauser _gamePauser;
    private readonly CompositeDisposable _compositeDisposable = new();

    public PauseMenuSwitchGamePauserMediator(PauseMenuSwitch pauseMenuSwitch, GamePauser gamePauser)
    {
        _pauseMenuSwitch = pauseMenuSwitch;
        _gamePauser = gamePauser;
    }

    public void Initialize()
    {
        _pauseMenuSwitch.PauseMenuOpened
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
