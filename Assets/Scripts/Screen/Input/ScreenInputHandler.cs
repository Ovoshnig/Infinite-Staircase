using R3;
using System;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class ScreenInputHandler : IInitializable, IDisposable
{
    private readonly Subject<bool> _isSwitchFullScreenPressed = new();
    private readonly Subject<bool> _isPassSplashImagePressed = new();
    private InputActionMap _actionMap;

    public Observable<bool> IsSwitchFullScreenPressed => _isSwitchFullScreenPressed;
    public Observable<bool> IsPassSplashImagePressed => _isPassSplashImagePressed;

    public void Initialize()
    {
        PlayerInput playerInput = new();
        PlayerInput.ScreenActions screenActions = playerInput.Screen;
        _actionMap = InputSystem.actions.FindActionMap(nameof(playerInput.Screen));

        _actionMap.FindAction(nameof(screenActions.SwitchFullScreen)).performed += OnFullScreenSwitch;
        _actionMap.FindAction(nameof(screenActions.SwitchFullScreen)).canceled += OnFullScreenSwitch;
        _actionMap.FindAction(nameof(screenActions.PassSplashImage)).performed += OnPassSplashImage;
        _actionMap.FindAction(nameof(screenActions.PassSplashImage)).canceled += OnPassSplashImage;

        _actionMap.Enable();
    }

    public void Dispose() => _actionMap.Disable();

    private void OnFullScreenSwitch(InputAction.CallbackContext context) =>
        _isSwitchFullScreenPressed.OnNext(context.ReadValueAsButton());

    private void OnPassSplashImage(InputAction.CallbackContext context) =>
        _isPassSplashImagePressed.OnNext(context.ReadValueAsButton());
}
