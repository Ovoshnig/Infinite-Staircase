using System.Linq;
using UnityEngine.InputSystem;

public class Vector2KeyBinder : KeyBinder
{
    private static readonly string[] _directionNames =
    {
        KeyBindingConstants.UpName,
        KeyBindingConstants.LeftName,
        KeyBindingConstants.DownName,
        KeyBindingConstants.RightName
    };

    private InputControl[] _temporaryControls;
    private int _keyInputNumber;

    public Vector2KeyBinder(KeyListeningTracker listeningTracker, SettingsStorage settingsStorage,
        InputAction inputAction) :
        base(listeningTracker, settingsStorage, inputAction)
    {
    }

    public override string ActionDisplayName
    {
        get
        {
            InputControl[] controls = InputAction.controls.ToArray();

            if (controls.Length >= 4)
                (controls[2], controls[1]) = (controls[1], controls[2]);

            string displayName = string.Join("/", controls.Select(c =>
            {
                string name = c.name;
                name = char.ToUpper(name[0]) + name[1..];
                return name;
            }));

            return displayName;
        }
    }

    public override string WaitInputText => $"ќжидание ввода {_directionNames[_keyInputNumber]} клавиши...";

    public override void StartListening()
    {
        if (!ListeningTracker.TryStartListening())
            return;

        _keyInputNumber = 0;
        _temporaryControls = new InputControl[4];

        base.StartListening();
    }

    public override void ApplyBindingOverrides()
    {
        for (int i = 0; i < 4; i++)
            InputAction.ApplyBindingOverride(i + 1, _temporaryControls[i].path);

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
            _temporaryControls[_keyInputNumber] = control;
            _keyInputNumber++;

            if (_keyInputNumber >= 4)
                HandleInput();
            else
                NotifyInputWaiting();
        }
    }

    protected override void HandleInput()
    {
        (_temporaryControls[1], _temporaryControls[2]) = (_temporaryControls[2], _temporaryControls[1]);

        bool hasDuplicates = _temporaryControls
            .GroupBy(c => c.path)
            .Any(g => g.Count() > 1);
        bool sameAsDefault = true;
        bool sameAsCurrent = true;

        for (int i = 0; i < 4; i++)
        {
            string defaultBindingPath = InputAction.bindings[i + 1].path;
            string currentBindingPath = InputAction.bindings[i + 1].effectivePath;

            if (!InputControlPath.Matches(defaultBindingPath, _temporaryControls[i]))
                sameAsDefault = false;

            if (!InputControlPath.Matches(currentBindingPath, _temporaryControls[i]))
                sameAsCurrent = false;
        }

        if (!hasDuplicates && !sameAsCurrent)
        {
            if (sameAsDefault)
                RemoveBindingOverrides();
            else
                ApplyBindingOverrides();
        }

        StopListening();
    }
}
