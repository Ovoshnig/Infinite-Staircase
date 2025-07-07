using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
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
            .Where(_ => _cameraSwitch.IsFirstPerson.CurrentValue)
            .Subscribe(value =>
            {
                if (value)
                    _playerScopeView.Disable();
                else
                    _playerScopeView.Enable();
            })
            .AddTo(CompositeDisposable);
    }
}
