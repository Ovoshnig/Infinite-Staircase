using System;
using System.Linq;
using TMPro;
using UnityEngine;
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

    public Vector2BindingHandler(KeyBindingsTracker bindingsTracker, TMP_Text bindingText,
        Color normalTextColor, Color waitingTextColor, PlayerInput playerInput, InputAction inputAction) :
        base(bindingsTracker, bindingText, normalTextColor, waitingTextColor, playerInput, inputAction)
    {
    }

    public override void StartListening()
    {
        if (!BindingsTracker.TryStartListening())
            return;

        base.StartListening();

        _keyInputNumber = 0;
        _temporaryControls = new InputControl[4];
        DisplayWaitingMessage();
    }

    protected override void OnAnyKeyPerformed(InputAction.CallbackContext _)
    {
        var pressedControl = BindingsTracker.AllControls.First(c => c.IsPressed());

        if (pressedControl == Keyboard.current.escapeKey)
        {
            CancelBinding();
        }
        else
        {
            _temporaryControls[_keyInputNumber] = pressedControl;
            _keyInputNumber++;

            if (_keyInputNumber == 4)
                CompleteBinding(pressedControl);
            else
                DisplayWaitingMessage();
        }
    }

    protected override void CompleteBinding(InputControl control)
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
                Reset();
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    InputAction action = PlayerInputProperty.FindAction(InputActionProperty.name);
                    InputActionProperty.ApplyBindingOverride(i + 1, _temporaryControls[i].path);
                    action.ApplyBindingOverride(i + 1, _temporaryControls[i].path);
                }
            }
        }

        base.CompleteBinding(control);
    }

    protected override string GetActionDisplayName()
    {
        var controls = InputActionProperty.controls.ToArray();

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

    private void DisplayWaitingMessage()
    {
        var ordinalNumber = Enum.GetValues(typeof(Vector2Directions)).GetValue(_keyInputNumber);
        BindingText.text = $"ќжидание ввода {ordinalNumber} клавиши...";
    }
}
