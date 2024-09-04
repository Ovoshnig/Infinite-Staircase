using R3;
using System;
using UnityEngine.InputSystem;
using Zenject;

public class ScreenInputHandler : IInitializable, IDisposable
{
    private readonly PlayerInput _playerInput = new();
    private readonly ReactiveProperty<bool> _isSwitchFullScreenPressed = new(false);
    private readonly ReactiveProperty<bool> _isPassSplashImagePressed = new(false);

    public ReadOnlyReactiveProperty<bool> IsSwitchFullScreenPressed => _isSwitchFullScreenPressed;
    public ReadOnlyReactiveProperty<bool> IsPassSplashImagePressed => _isPassSplashImagePressed;

    public void Initialize()
    {
        _playerInput.Screen.SwitchFullScreen.performed += OnFullScreenSwitch;
        _playerInput.Screen.SwitchFullScreen.canceled += OnFullScreenSwitch;
        _playerInput.Screen.PassSplashImage.performed += OnPassSplashImage;
        _playerInput.Screen.PassSplashImage.canceled += OnPassSplashImage;

        _playerInput.Enable();
    }

    public void Dispose() => _playerInput.Disable();

    private void OnFullScreenSwitch(InputAction.CallbackContext context) =>
        _isSwitchFullScreenPressed.Value = context.ReadValueAsButton();

    private void OnPassSplashImage(InputAction.CallbackContext context) =>
        _isPassSplashImagePressed.Value = context.ReadValueAsButton();
}
