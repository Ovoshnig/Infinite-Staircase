using UnityEngine.InputSystem;

public class AxisKeyBinder : CompositeKeyBinder
{
    private static readonly string[] _directionNames = {
        KeyBindingConstants.NegativeName, 
        KeyBindingConstants.PositiveName
    };

    public AxisKeyBinder(ButtonListener listener, SettingsStorage storage, InputAction action)
        : base(listener, storage, action) { }

    protected override int RequiredInputsCount => 2;
    protected override string[] DirectionNames => _directionNames;
}
