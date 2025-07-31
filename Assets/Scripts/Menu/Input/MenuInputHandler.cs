using R3;

public class MenuInputHandler : InputHandler<InputActions.MenuActions>
{
    public MenuInputHandler(InputActions inputActions)
        : base(inputActions.Menu) { }

    public ReadOnlyReactiveProperty<bool> CloseCurrentPressed { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        CloseCurrentPressed = BindButton(a => a.CloseCurrent);
    }

    protected override void EnableActions() => Actions.Enable();

    protected override void DisableActions() => Actions.Disable();
}
