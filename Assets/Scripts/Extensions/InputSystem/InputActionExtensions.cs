using System;
using UnityEngine.InputSystem;

public static class InputActionExtensions
{
    public static InputAction Subscribe(this InputAction action, Action<InputAction.CallbackContext> handler)
    {
        action.performed += handler;
        action.canceled += handler;

        return action;
    }

    public static InputAction Unsubscribe(this InputAction action, Action<InputAction.CallbackContext> handler)
    {
        action.performed -= handler;
        action.canceled -= handler;

        return action;
    }
}
