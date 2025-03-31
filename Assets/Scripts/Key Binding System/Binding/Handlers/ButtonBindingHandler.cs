using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonBindingHandler : BindingHandler
{
    public ButtonBindingHandler(KeyBindingsTracker bindingsTracker, TMP_Text bindingText,
        Color normalTextColor, Color waitingTextColor, PlayerInput playerInput, InputAction inputAction) :
        base(bindingsTracker, bindingText, normalTextColor, waitingTextColor, playerInput, inputAction)
    {
    }

    public override void StartListening()
    {
        if (!BindingsTracker.TryStartListening())
            return;

        base.StartListening();

        BindingText.text = InputConstants.WaitInputText;
    }

    protected override void OnAnyKeyPerformed(InputAction.CallbackContext _)
    {
        InputControl pressedControl = BindingsTracker.AllControls.First(c => c.IsPressed());

        if (pressedControl == Keyboard.current.escapeKey)
            CancelBinding();
        else
            CompleteBinding(pressedControl);
    }

    protected override void CompleteBinding(InputControl control)
    {
        if (InputActionProperty.controls[0].path != control.path)
        {
            string defaultControlName = InputActionProperty.bindings[0].path.Split('/')[^1];
            string newControlName = control.path.Split('/')[^1];

            if (defaultControlName == newControlName)
            {
                Reset();
            }
            else
            {
                InputActionProperty.ApplyBindingOverride(control.path);
                InputAction action = PlayerInputProperty.FindAction(InputActionProperty.name);
                action.ApplyBindingOverride(control.path);
            }
        }
        
        base.CompleteBinding(control);
    }

    protected override string GetActionDisplayName()
    {
        string actionName = InputActionProperty.controls[0].name;
        actionName = char.ToUpper(actionName[0]) + actionName[1..];

        return actionName;
    }
}
