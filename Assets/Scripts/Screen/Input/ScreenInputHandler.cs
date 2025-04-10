using R3;
using System;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class ScreenInputHandler : IInitializable, IDisposable
{
    private readonly InputActions _inputActions;
    private readonly ReactiveProperty<bool> _isSwitchFullScreenPressed = new(false);
    private readonly ReactiveProperty<bool> _isPassSplashImagePressed = new(false);
    private InputActions.ScreenActions _screenActions;

    public ScreenInputHandler(InputActions inputActions) => _inputActions = inputActions;

    public ReadOnlyReactiveProperty<bool> IsSwitchFullScreenPressed => _isSwitchFullScreenPressed;
    public ReadOnlyReactiveProperty<bool> IsPassSplashImagePressed => _isPassSplashImagePressed;

    public void Initialize()
    {
        _screenActions = _inputActions.Screen;
        _screenActions.Enable();

        _screenActions.SwitchFullScreen.Subscribe(OnFullScreenSwitch);
        _screenActions.PassSplashImage.Subscribe(OnPassSplashImage);
    }

    public void Dispose()
    {
        _screenActions.Disable();

        _screenActions.SwitchFullScreen.Unsubscribe(OnFullScreenSwitch);
        _screenActions.PassSplashImage.Unsubscribe(OnPassSplashImage);
    }

    private void OnFullScreenSwitch(InputAction.CallbackContext context) => 
        _isSwitchFullScreenPressed.Value = context.ReadValueAsButton();

    private void OnPassSplashImage(InputAction.CallbackContext context) =>
        _isPassSplashImagePressed.Value = context.ReadValueAsButton();
}
