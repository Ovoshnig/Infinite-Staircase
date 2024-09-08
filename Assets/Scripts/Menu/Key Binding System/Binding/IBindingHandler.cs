using TMPro;
using UnityEngine.InputSystem;

public interface IBindingHandler
{
    void Bind(InputAction inputAction, TMP_Text bindingText);
    void Reset(InputAction inputAction, TMP_Text bindingText);
}
