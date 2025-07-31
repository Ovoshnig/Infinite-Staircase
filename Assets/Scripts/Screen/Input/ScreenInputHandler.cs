using R3;

public class ScreenInputHandler : InputHandler<InputActions.ScreenActions>
{
    public ScreenInputHandler(InputActions inputActions)
        : base(inputActions.Screen) { }

    public ReadOnlyReactiveProperty<bool> SwitchFullScreenPressed { get; private set; }
    public ReadOnlyReactiveProperty<bool> PassSplashImagePressed { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        SwitchFullScreenPressed = BindButton(a => a.SwitchFullScreen);
        PassSplashImagePressed = BindButton(a => a.PassSplashImage);
    }

    protected override void EnableActions() => Actions.Enable();

    protected override void DisableActions() => Actions.Disable();
}
