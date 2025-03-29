using R3;
using System;
using UnityEngine.InputSystem;
using VContainer.Unity;
using static PlayerInput;

public class ScreenInputHandler : IInitializable, IDisposable
{
    private readonly SceneSwitch _sceneSwitch;
    private readonly ReactiveProperty<bool> _isSwitchFullScreenPressed = new(false);
    private readonly ReactiveProperty<bool> _isPassSplashImagePressed = new(false);
    private readonly CompositeDisposable _compositeDisposable = new();
    private InputActionMap _actionMap;
    private InputAction _switchFullScreenAction;
    private InputAction _passSplashImageAction;

    public ScreenInputHandler(SceneSwitch sceneSwitch) => _sceneSwitch = sceneSwitch;

    public ReadOnlyReactiveProperty<bool> IsSwitchFullScreenPressed => _isSwitchFullScreenPressed;
    public ReadOnlyReactiveProperty<bool> IsPassSplashImagePressed => _isPassSplashImagePressed;

    public void Initialize()
    {
        PlayerInput playerInput = new();
        ScreenActions screenActions = playerInput.Screen;
        _actionMap = InputSystem.actions.FindActionMap(nameof(playerInput.Screen));

        _switchFullScreenAction = _actionMap.FindAction(screenActions.SwitchFullScreen.name);
        _passSplashImageAction = _actionMap.FindAction(screenActions.PassSplashImage.name);

        _switchFullScreenAction.performed += OnFullScreenSwitch;
        _switchFullScreenAction.canceled += OnFullScreenSwitch;
        _passSplashImageAction.performed += OnPassSplashImage;
        _passSplashImageAction.canceled += OnPassSplashImage;

        _sceneSwitch.IsSceneLoading
            .Where(value => !value)
            .Subscribe(value => OnSceneLoaded())
            .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _actionMap.Dispose();

        _compositeDisposable?.Dispose();
    }

    private void OnSceneLoaded()
    {
        _actionMap.Enable();
        _switchFullScreenAction.Enable();
        _passSplashImageAction.Enable();
    }

    private void OnFullScreenSwitch(InputAction.CallbackContext context) => 
        _isSwitchFullScreenPressed.Value = context.ReadValueAsButton();

    private void OnPassSplashImage(InputAction.CallbackContext context) =>
        _isPassSplashImagePressed.Value = context.ReadValueAsButton();
}
