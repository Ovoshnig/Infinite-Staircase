using UnityEngine.InputSystem;

public class Vector2KeyBinder : CompositeKeyBinder
{
    private static readonly string[] _directionNames = {
        KeyBindingConstants.UpName,
        KeyBindingConstants.DownName,
        KeyBindingConstants.LeftName,
        KeyBindingConstants.RightName
    };

    public Vector2KeyBinder(KeyListeningTracker tracker, SettingsStorage storage, InputAction action)
        : base(tracker, storage, action) { }

    protected override int RequiredInputsCount => 4;
    protected override string[] DirectionNames => _directionNames;
}
