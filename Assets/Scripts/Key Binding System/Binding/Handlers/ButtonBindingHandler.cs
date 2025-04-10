using UnityEngine.InputSystem;

public class ButtonBindingHandler : BindingHandler
{
    public ButtonBindingHandler(KeyListeningTracker listeningTracker,
        InputActions inputActions, InputAction inputAction) :
        base(listeningTracker, inputActions, inputAction)
    {
    }

    protected override string WaitInputText => InputConstants.WaitInputText;

    public override void StartListening()
    {
        if (!ListeningTracker.TryStartListening())
            return;

        base.StartListening();
    }

    public override string GetActionDisplayName()
    {
        string actionName = InputActionProperty.controls[0].name;
        actionName = char.ToUpper(actionName[0]) + actionName[1..];

        return actionName;
    }

    protected override void OnAnyButtonPressed(InputControl control)
    {
        if (control.device is not Keyboard)
            return;

        if (control == Keyboard.current.escapeKey)
            CancelListening();
        else
            ApplyBinding(control);
    }
     
    protected override void ApplyBinding(InputControl control)
    {
        if (InputActionProperty.controls[0].path != control.path)
        {
            string defaultControlName = InputActionProperty.bindings[0].path.Split('/')[^1];
            string newControlName = control.path.Split('/')[^1];

            if (defaultControlName == newControlName)
            {
                ResetBinding();
            }
            else
            {
                InputActionProperty.ApplyBindingOverride(control.path);
                InputAction action = InputActionsProperty.FindAction(InputActionProperty.name);
                action.ApplyBindingOverride(control.path);
            }
        }
        
        base.ApplyBinding(control);
    }
}
