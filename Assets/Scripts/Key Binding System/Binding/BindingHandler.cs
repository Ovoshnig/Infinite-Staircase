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

        _anyKeyInputAction = new InputAction(type: InputActionType.Button);
        Initialize();
    }

    protected KeyBindingsTracker BindingsTracker => _bindingsTracker;
    protected TMP_Text BindingText => _bindingText;
    protected InputAction InputAction => _inputAction;

    public void Initialize()
    {
        _bindingText.text = GetActionDisplayName();

        _anyKeyInputAction.AddBinding(InputConstants.KeyboardAnyKeyPath);
        _anyKeyInputAction.performed += OnAnyKeyPerformed;
    }

    public virtual void StartListening()
    {
        _anyKeyInputAction.Enable();
        _bindingText.color = _waitingTextColor;
    }

    public virtual void Reset()
    {
        _inputAction.RemoveAllBindingOverrides();
        _bindingText.text = GetActionDisplayName();
    }

    protected abstract void OnAnyKeyPerformed(InputAction.CallbackContext _);

    protected virtual void CompleteBinding(InputControl _)
    {
        _bindingText.text = GetActionDisplayName();

        StopListening();
    }

    protected void CancelBinding()
    {
        _bindingText.text = GetActionDisplayName();

        StopListening();
    }

    protected virtual void StopListening()
    {
        _bindingText.color = _normalTextColor;

        _anyKeyInputAction.Disable();
        _bindingsTracker.StopListening();
    }

    protected abstract string GetActionDisplayName();
}
