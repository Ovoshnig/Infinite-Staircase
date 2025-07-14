using UnityEngine.InputSystem;

public class ButtonKeyBinder : KeyBinder
{
    public ButtonKeyBinder(KeyListeningTracker listeningTracker, SettingsStorage settingsStorage,
        InputActions inputActions, InputAction inputAction) :
        base(listeningTracker, settingsStorage, inputActions, inputAction)
    {
    }

    protected override string WaitInputText => KeyBindingConstants.WaitInputText;

    public override void StartListening()
    {
        if (!ListeningTracker.TryStartListening())
            return;

        base.StartListening();
    }

    public override string GetActionDisplayName()
    {
        string actionName = InputAction.controls[0].name;
        actionName = char.ToUpper(actionName[0]) + actionName[1..];
        return actionName;
    }

    protected override void OnAnyButtonPressed(InputControl control)
    {
        if (control == Keyboard.current.escapeKey)
            CancelListening();
        else
            ApplyBinding(control);
    }

    protected override void ApplyBinding(InputControl control)
    {
        if (InputAction.controls[0].path != control.path)
        {
            string defaultControlName = InputAction.bindings[0].path.Split('/')[^1];
            string newControlName = control.path.Split('/')[^1];

            if (defaultControlName == newControlName)
            {
                ResetBinding();
            }
            else
            {
                InputAction.ApplyBindingOverride(control.path);
                InputAction action = InputActions.FindAction(InputAction.name);
                action.ApplyBindingOverride(control.path);

                EnableOverrides();
            }
        }

        base.ApplyBinding(control);
    }
}
