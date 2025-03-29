using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonBindingHandler : BindingHandler
{
    public ButtonBindingHandler(KeyBindingsTracker bindingsTracker, TMP_Text bindingText,
        Color normalTextColor, Color waitingTextColor, InputAction inputAction) :
        base(bindingsTracker, bindingText, normalTextColor, waitingTextColor, inputAction)
    {
    }

    public override void StartListening()
    {
        if (!BindingsTracker.TryStartListening())
            return;

        base.StartListening();

        BindingText.text = "ќжидание ввода...";
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
        if (InputAction.controls[0].path != control.path)
            InputAction.ApplyBindingOverride(control.path);

        base.CompleteBinding(control);
    }

    protected override string GetActionDisplayName()
    {
        string actionName = InputAction.controls[0].name;
        actionName = char.ToUpper(actionName[0]) + actionName[1..];

        return actionName;
    }
}
