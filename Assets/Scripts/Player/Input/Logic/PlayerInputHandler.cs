using R3;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : InputHandler<InputActions.PlayerActions>
{
    public PlayerInputHandler(InputActions inputActions)
        : base(inputActions.Player) { }

    public InputAction LookAction => Actions.Look;
    public InputAction ZoomAction => Actions.Zoom;
    public ReadOnlyReactiveProperty<Vector2> WalkInput { get; private set; }
    public ReadOnlyReactiveProperty<Vector2> LookInput { get; private set; }
    public ReadOnlyReactiveProperty<Vector2> ZoomInput { get; private set; }
    public ReadOnlyReactiveProperty<bool> RunPressed { get; private set; }
    public ReadOnlyReactiveProperty<bool> JumpPressed { get; private set; }
    public ReadOnlyReactiveProperty<bool> TogglePerspectivePressed { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        WalkInput = BindValue(a => a.Walk);
        LookInput = BindValue(a => a.Look);
        ZoomInput = BindValue(a => a.Zoom);
        RunPressed = BindButton(a => a.Run);
        JumpPressed = BindButton(a => a.Jump);
        TogglePerspectivePressed = BindButton(a => a.TogglePerspective);
    }

    protected override void EnableActions() => Actions.Enable();

    protected override void DisableActions() => Actions.Disable();
}
