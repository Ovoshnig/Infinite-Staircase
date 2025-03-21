using R3;
using System;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class CameraSwitch : IInitializable, IDisposable
{
    private readonly ReactiveProperty<bool> _isFirstPerson = new(true);
    private readonly ReactiveProperty<bool> _isScopeEnabled = new(false);
    private readonly CompositeDisposable _compositeDisposable = new();
    private CinemachineCamera _firstPersonCamera;
    private CinemachineCamera _thirdPersonCamera;
    private PlayerInputHandler _playerInputHandler;
    private WindowTracker _windowTracker;

    [Inject]
    public void Construct(PlayerInputHandler playerInputHandler, WindowTracker windowTracker,
        CharacterController characterController)
    {
        _playerInputHandler = playerInputHandler;
        _windowTracker = windowTracker;

        _firstPersonCamera = characterController
            .GetComponentInChildren<CinemachineHardLockToTarget>()
            .gameObject
            .GetComponent<CinemachineCamera>();

        _thirdPersonCamera = characterController
            .GetComponentInChildren<CinemachineOrbitalFollow>()
            .gameObject
            .GetComponent<CinemachineCamera>();
    }

    public ReadOnlyReactiveProperty<bool> IsFirstPerson => _isFirstPerson;
    public ReadOnlyReactiveProperty<bool> IsScopeEnabled => _isScopeEnabled;

    public void Initialize()
    {
        _playerInputHandler.IsTogglePerspectivePressed
            .Where(value => value)
            .Subscribe(_ => _isFirstPerson.Value = !_isFirstPerson.Value)
            .AddTo(_compositeDisposable);

        _isFirstPerson
            .Subscribe(value => SetCamera(value))
            .AddTo(_compositeDisposable);

        _windowTracker.IsOpen
            .Where(_ => _isFirstPerson.Value)
            .Subscribe(value => _isScopeEnabled.Value = !value)
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();

    private void SetCamera(bool isFirstPerson)
    {
        _firstPersonCamera.Priority = isFirstPerson ? 1 : 0;
        _thirdPersonCamera.Priority = isFirstPerson ? 0 : 1;

        _isScopeEnabled.Value = isFirstPerson;
    }
}
