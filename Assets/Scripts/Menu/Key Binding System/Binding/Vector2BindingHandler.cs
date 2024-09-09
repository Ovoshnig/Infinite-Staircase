using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Vector2BindingHandler : BindingHandler
{
    public Vector2BindingHandler(KeyBindingsTracker bindingsTracker, TMP_Text bindingText, 
        Color normalTextColor, Color waitingTextColor, InputAction inputAction) : 
        base(bindingsTracker, bindingText, normalTextColor, waitingTextColor, inputAction)
    {
    }

    public override void Bind()
    {
        base.Bind();

        Debug.Log("Bind Vector2");
    }
}
