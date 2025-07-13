using R3;
using System;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class ScreenInputHandler : IInitializable, IDisposable
{
    private readonly InputActions.ScreenActions _screenActions;
    private readonly ReactiveProperty<bool> _isSwitchFullScreenPressed = new(false);
    private readonly ReactiveProperty<bool> _isPassSplashImagePressed = new(false);

    public ScreenInputHandler(InputActions inputActions) => _screenActions = inputActions.Screen;

    public ReadOnlyReactiveProperty<bool> IsSwitchFullScreenPressed => _isSwitchFullScreenPressed;
    public ReadOnlyReactiveProperty<bool> IsPassSplashImagePressed => _isPassSplashImagePressed;

    public void Initialize()
    {
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
