using UnityEngine.InputSystem;

public class ButtonKeyBinder : KeyBinder
{
    public ButtonKeyBinder(KeyListeningTracker tracker, SettingsStorage storage, InputAction action)
        : base(tracker, storage, action) { }

    protected override int RequiredInputsCount => 1;

    protected override string GetWaitingText(int _) => KeyBindingConstants.WaitInputText;

    protected override void ApplyBindingOverrides(InputControl[] controls) =>
        InputAction.ApplyBindingOverride(0, controls[0].path);
}
