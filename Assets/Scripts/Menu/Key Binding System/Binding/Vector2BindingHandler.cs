using TMPro;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

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
        Color normalTextColor, Color waitingTextColor, InputAction inputAction) : 
        base(bindingsTracker, bindingText, normalTextColor, waitingTextColor, inputAction)
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
        try
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
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    protected override void CompleteBinding(InputControl control)
    {
        (_temporaryControls[1], _temporaryControls[2]) = (_temporaryControls[2], _temporaryControls[1]);

        for (int i = 0; i < 4; i++)
            InputAction.ApplyBindingOverride(i + 1, _temporaryControls[i].path);

        base.CompleteBinding(control);
    }

    protected override string GetActionDisplayName()
    {
        var controls = InputAction.controls.ToArray();

        if (controls.Length == 4)
            (controls[2], controls[1]) = (controls[1], controls[2]);

        var displayName = string.Join("/", controls.Select(c => c.name.FirstCharacterToUpper()));

        return displayName;
    }

    private void DisplayWaitingMessage()
    {
        var ordinalNumber = Enum.GetValues(typeof(Vector2Directions)).GetValue(_keyInputNumber);
        BindingText.text = $"ќжидание ввода {ordinalNumber} клавиши...";
    }
}
