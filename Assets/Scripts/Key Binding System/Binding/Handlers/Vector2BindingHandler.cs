using System;
using System.Linq;
using UnityEngine.InputSystem;

public class Vector2BindingHandler : BindingHandler
{
    private enum Vector2Directions
    {
        верхней = 1,
        левой,
        нижней,
        правой
    }

    private InputControl[] _temporaryControls;
    private int _keyInputNumber;

    public Vector2BindingHandler(KeyListeningTracker listeningTracker, 
        InputActions inputActions, InputAction inputAction) :
        base(listeningTracker, inputActions, inputAction)
    {
    }

    protected override string WaitInputText
    {
        get
        {
            object ordinalNumber = Enum.GetValues(typeof(Vector2Directions)).GetValue(_keyInputNumber);

            return $"ќжидание ввода {ordinalNumber} клавиши...";
        }
    }

    public override void StartListening()
    {
        if (!ListeningTracker.TryStartListening())
            return;

        _keyInputNumber = 0;
        _temporaryControls = new InputControl[4];

        base.StartListening();
    }

    public override string GetActionDisplayName()
    {
        InputControl[] controls = InputActionProperty.controls.ToArray();

        if (controls.Length == 4)
            (controls[2], controls[1]) = (controls[1], controls[2]);

        string displayName = string.Join("/", controls.Select(c =>
        {
            string name = c.name;
            name = char.ToUpper(name[0]) + name[1..];

            return name;
        }));

        return displayName;
    }

    protected override void OnAnyButtonPressed(InputControl control)
    {
        if (control.device is not Keyboard)
            return;

        if (control == Keyboard.current.escapeKey)
        {
            CancelListening();
        }
        else
        {
            _temporaryControls[_keyInputNumber] = control;
            _keyInputNumber++;

            if (_keyInputNumber == 4)
                ApplyBinding(control);
            else
                SetWaitingMessage();
        }
    }

    protected override void ApplyBinding(InputControl control)
    {
        (_temporaryControls[1], _temporaryControls[2]) = (_temporaryControls[2], _temporaryControls[1]);

        bool[] isDuplicates = _temporaryControls
            .GroupBy(c => c.path)
            .Select(g => g.Count() > 1)
            .ToArray();
        bool[] isChanges = new bool[4];
        bool[] sameAsDefault = new bool[4];

        for (int i = 0; i < 4; i++)
        {
            string defaultControlName = InputActionProperty.bindings[i + 1].path.Split('/')[^1];
            string currentControlName = InputActionProperty.bindings[i + 1].effectivePath.Split('/')[^1];
            string newControlName = _temporaryControls[i].path.Split('/')[^1];

            if (currentControlName != newControlName)
                isChanges[i] = true;

            if (newControlName == defaultControlName)
                sameAsDefault[i] = true;
        }

        if (isDuplicates.All(x => !x) && isChanges.Any(x => x))
        {
            if (sameAsDefault.All(x => x))
            {
                ResetBinding();
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    InputAction action = InputActionsProperty.FindAction(InputActionProperty.name);
                    InputActionProperty.ApplyBindingOverride(i + 1, _temporaryControls[i].path);
                    action.ApplyBindingOverride(i + 1, _temporaryControls[i].path);
                }
            }
        }

        base.ApplyBinding(control);
    }
}
