using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public static class InputControlExtensions
{
    public static string ToCaseIndependentString(this InputControl inputControl)
    {
        if (inputControl is KeyControl keyControl)
        {
            Key key = keyControl.keyCode;

            if (key >= Key.Backquote && key <= Key.Z)
            {
                InputAction tempAction = new(binding: $"<Keyboard>/{key}");
                return tempAction.bindings[0].ToDisplayString();
            }
            else
            {
                return inputControl.displayName;
            }
        }
        
        return inputControl.displayName;
    }
}
