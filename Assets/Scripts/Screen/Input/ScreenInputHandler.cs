using R3;
using System;
using UnityEngine.InputSystem;
using Zenject;

public class ScreenInputHandler : IInitializable, IDisposable
{
    private readonly PlayerInput _playerInput;
    private readonly Subject<bool> _isSwitchFullScreenPressed = new();
    private readonly Subject<bool> _isPassSplashImagePressed = new();

    [Inject]
    public ScreenInputHandler(PlayerInput playerInput) => _playerInput = playerInput;

    public Observable<bool> IsSwitchFullScreenPressed => _isSwitchFullScreenPressed;
    public Observable<bool> IsPassSplashImagePressed => _isPassSplashImagePressed;

    public void Initialize()
    {
        _playerInput.Screen.SwitchFullScreen.performed += OnFullScreenSwitch;
        _playerInput.Screen.SwitchFullScreen.canceled += OnFullScreenSwitch;
        _playerInput.Screen.PassSplashImage.performed += OnPassSplashImage;
        _playerInput.Screen.PassSplashImage.canceled += OnPassSplashImage;

        _playerInput.Screen.Enable();
    }


    public void Dispose() => _playerInput.Screen.Disable();

    private void OnFullScreenSwitch(InputAction.CallbackContext context) =>
        _isSwitchFullScreenPressed.OnNext(context.ReadValueAsButton());

    private void OnPassSplashImage(InputAction.CallbackContext context) =>
        _isPassSplashImagePressed.OnNext(context.ReadValueAsButton());
}
