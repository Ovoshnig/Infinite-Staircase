using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BindingHandler : IBindingHandler
{
    private readonly KeyBindingsTracker _bindingsTracker;
    private readonly InputAction _inputAction;
    private readonly InputAction _anyKeyInputAction;
    private readonly TMP_Text _bindingText;
    private readonly Color _normalTextColor;
    private readonly Color _waitingTextColor;

    protected BindingHandler(KeyBindingsTracker bindingsTracker, TMP_Text bindingText,
        Color normalTextColor, Color waitingTextColor, InputAction inputAction)
    {
        _bindingsTracker = bindingsTracker;
        _bindingText = bindingText;
        _normalTextColor = normalTextColor;
        _waitingTextColor = waitingTextColor;
        _inputAction = inputAction;

        _anyKeyInputAction = new InputAction(type: InputActionType.PassThrough);
        _anyKeyInputAction.AddBinding(InputConstants.KeyboardAnyKeyPath);
        _anyKeyInputAction.performed += OnAnyKeyPerformed;
    }

    protected TMP_Text BindingText => _bindingText;
    protected InputAction InputAction => _inputAction;

    public virtual void OnAnyKeyPerformed(InputAction.CallbackContext _)
    {
        var pressedControl = _bindingsTracker.AllControls.First(c => c.IsPressed());

        if (pressedControl == Keyboard.current.escapeKey)
            CancelBinding();
        else
            CompleteBinding(pressedControl);

        _bindingText.color = _normalTextColor;

        _anyKeyInputAction.Disable();
        _bindingsTracker.StopListening();
    }

    public virtual void Bind()
    {
        if (!_bindingsTracker.TryStartListening())
            return;

        _anyKeyInputAction.Enable();
        _bindingText.text = "ќжидание ввода...";
        _bindingText.color = _waitingTextColor;
    }

    public virtual void Reset()
    {
        _inputAction.RemoveBindingOverride(0);
        _bindingText.text = _inputAction.GetBindingDisplayString();
    }

    protected virtual void CompleteBinding(InputControl control)
    {
        _inputAction.ApplyBindingOverride(control.path);
        _bindingText.text = control.displayName;
    }

    private void CancelBinding() => _bindingText.text = _inputAction.GetBindingDisplayString();
}
