using UnityEngine.InputSystem;

public class ButtonKeyBinder : KeyBinder
{
    public ButtonKeyBinder(KeyListeningTracker listeningTracker, SettingsStorage settingsStorage,
        InputAction inputAction) :
        base(listeningTracker, settingsStorage, inputAction)
    {
    }

    public override string ActionDisplayName
    {
        get
        {
            string actionName = InputAction.controls[0].name;
            actionName = char.ToUpper(actionName[0]) + actionName[1..];
            return actionName;
        }
    }

    public override string WaitInputText => KeyBindingConstants.WaitInputText;

    public override void StartListening()
    {
        if (!ListeningTracker.TryStartListening())
            return;

        base.StartListening();
    }

    protected override void OnAnyButtonPressed(InputControl control)
    {
        if (control == Keyboard.current.escapeKey)
            StopListening();
        else
            ApplyBinding(control);
    }

    protected override void ApplyBinding(InputControl control)
    {
        string defaultBindingPath = InputAction.bindings[0].path;
        string currentBindingPath = InputAction.bindings[0].effectivePath;

        bool sameAsDefault = InputControlPath.Matches(defaultBindingPath, control);
        bool sameAsCurrent = InputControlPath.Matches(currentBindingPath, control);

        if (!sameAsCurrent)
        {
            if (sameAsDefault)
            {
                ResetBinding();
            }
            else
            {
                InputAction.ApplyBindingOverride(control.path);

                EnableOverrides();
            }
        }

        StopListening();
    }
}
