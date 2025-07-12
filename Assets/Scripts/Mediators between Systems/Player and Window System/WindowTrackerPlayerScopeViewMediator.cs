using R3;

public class WindowTrackerPlayerScopeViewMediator : Mediator
{
    private readonly WindowTracker _windowTracker;
    private readonly PlayerScopeView _playerScopeView;
    private readonly CameraSwitch _cameraSwitch;

    public WindowTrackerPlayerScopeViewMediator(WindowTracker windowTracker, 
        PlayerScopeView playerScopeView, CameraSwitch cameraSwitch)
    {
        _windowTracker = windowTracker;
        _playerScopeView = playerScopeView;
        _cameraSwitch = cameraSwitch;
    }

    public override void Initialize()
    {
        _windowTracker.IsOpen
            .Subscribe(OnWindowOpen)
            .AddTo(CompositeDisposable);
    }
    
    private void OnWindowOpen(bool isOpen)
    {
        if (!_cameraSwitch.IsFirstPerson.CurrentValue)
            return;

        if (isOpen)
            _playerScopeView.Disable();
        else
            _playerScopeView.Enable();
    }
}
