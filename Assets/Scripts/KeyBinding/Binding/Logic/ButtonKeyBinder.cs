using UnityEngine.InputSystem;

public class ButtonKeyBinder : KeyBinder
{
    private InputControl _inputControl;

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

        _inputControl = null;

        base.StartListening();
    }

    public override void ApplyBindingOverrides()
    {
        InputAction.ApplyBindingOverride(_inputControl.path);

        base.ApplyBindingOverrides();
    }

    protected override void OnAnyButtonPressed(InputControl control)
    {
        if (control == Keyboard.current.escapeKey)
        {
            StopListening();
        }
        else
        {
            _inputControl = control;
            HandleInput();
        }
    }

    protected override void HandleInput()
    {
        string defaultBindingPath = InputAction.bindings[0].path;
        string currentBindingPath = InputAction.bindings[0].effectivePath;

        bool sameAsDefault = InputControlPath.Matches(defaultBindingPath, _inputControl);
        bool sameAsCurrent = InputControlPath.Matches(currentBindingPath, _inputControl);

        if (!sameAsCurrent)
        {
            if (sameAsDefault)
                RemoveBindingOverrides();
            else
                ApplyBindingOverrides();
        }

        StopListening();
    }
}
