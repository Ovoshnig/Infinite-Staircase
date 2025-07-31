using R3;

public class WindowInputHandler : InputHandler<InputActions.WindowActions>
{
    public WindowInputHandler(InputActions inputActions)
        : base(inputActions.Window) { }

    public ReadOnlyReactiveProperty<bool> CloseCurrentPressed { get; private set; }
    public ReadOnlyReactiveProperty<bool> PauseMenuSwitchPressed { get; private set; }
    public ReadOnlyReactiveProperty<bool> InventorySwitchPressed { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        CloseCurrentPressed = BindButton(a => a.CloseCurrent);
        PauseMenuSwitchPressed = BindButton(a => a.SwitchPauseMenu);
        InventorySwitchPressed = BindButton(a => a.SwitchInventory);
    }

    protected override void EnableActions() => Actions.Enable();

    protected override void DisableActions() => Actions.Disable();
}
