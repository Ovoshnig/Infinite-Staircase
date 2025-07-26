using UnityEngine.InputSystem;

public class AxisKeyBinder : CompositeKeyBinder
{
    private static readonly string[] _directionNames = {
        KeyBindingConstants.NegativeName, 
        KeyBindingConstants.PositiveName
    };

    public AxisKeyBinder(KeyListeningTracker tracker, SettingsStorage storage, InputAction action)
        : base(tracker, storage, action) { }

    protected override int RequiredInputsCount => 2;
    protected override string[] DirectionNames => _directionNames;
}
