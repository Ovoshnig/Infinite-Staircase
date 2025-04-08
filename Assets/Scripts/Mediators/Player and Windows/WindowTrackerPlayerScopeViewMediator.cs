using R3;
using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

[RequireComponent(typeof(Image))]
public class WindowTrackerPlayerScopeViewMediator : IInitializable, IDisposable
{
    private readonly WindowTracker _windowTracker;
    private readonly PlayerScopeView _playerScopeView;
    private readonly CameraSwitch _cameraSwitch;
    private readonly CompositeDisposable _compositeDisposable = new();

    public WindowTrackerPlayerScopeViewMediator(WindowTracker windowTracker, 
        PlayerScopeView playerScopeView, CameraSwitch cameraSwitch)
    {
        _windowTracker = windowTracker;
        _playerScopeView = playerScopeView;
        _cameraSwitch = cameraSwitch;
    }

    public void Initialize()
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
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
