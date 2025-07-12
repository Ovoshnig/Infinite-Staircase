using R3;
using System;
using System.Linq;
using UnityEngine.InputSystem;
using VContainer.Unity;

public abstract class KeyBinder : IKeyBinder, IInitializable, IDisposable
{
    private readonly KeyListeningTracker _listeningTracker;
    private readonly InputActions _inputActions;
    private readonly InputAction _inputAction;
    private readonly ReactiveProperty<bool> _hasOverrides = new(false);
    private readonly ReactiveProperty<string> _bindingText = new("Key");
    private readonly ReactiveProperty<bool> _isListening = new(false);
    private readonly CompositeDisposable _compositeDisposable = new();

    public KeyBinder(KeyListeningTracker listeningTracker, 
        InputActions InputActions, InputAction inputAction)
    {
        _listeningTracker = listeningTracker;
        _inputActions = InputActions;
        _inputAction = inputAction;
    }

    public ReadOnlyReactiveProperty<bool> HasOverrides => _hasOverrides;
    public ReadOnlyReactiveProperty<string> BindingText => _bindingText;
    public ReadOnlyReactiveProperty<bool> IsListening => _isListening;

    protected KeyListeningTracker ListeningTracker => _listeningTracker;
    protected InputActions InputActions => _inputActions;
    protected InputAction InputAction => _inputAction;
    protected abstract string WaitInputText { get; }

    public virtual void Initialize()
    {
        _hasOverrides.Value = _inputAction.bindings.Any(b => b.hasOverrides);

        _bindingText.Value = GetActionDisplayName();
    }

    public virtual void Dispose() => _compositeDisposable?.Dispose();

    public virtual void StartListening()
    {
        InputSystem.onAnyButtonPress
            .ToObservable()
            .Where(control => control.device is Keyboard)
            .Subscribe(OnAnyButtonPressed)
            .AddTo(_compositeDisposable);

        _isListening.Value = true;
        SetWaitingMessage();
    }

    public virtual void ResetBinding()
    {
        _hasOverrides.Value = false;

        _inputAction.RemoveAllBindingOverrides();
        _inputActions.RemoveAllBindingOverrides();
        _bindingText.Value = GetActionDisplayName();
    }

    public abstract string GetActionDisplayName();

    public void EnableOverrides() => _hasOverrides.Value = true;

    public virtual void CancelListening()
    {
        _bindingText.Value = GetActionDisplayName();

        StopListening();
    }

    protected abstract void OnAnyButtonPressed(InputControl control);

    protected virtual void ApplyBinding(InputControl _)
    {
        _bindingText.Value = GetActionDisplayName();

        StopListening();
    }

    protected virtual void StopListening()
    {
        _compositeDisposable.Clear();
        _listeningTracker.StopListening();
        _isListening.Value = false;
    }

    protected void SetWaitingMessage() => _bindingText.Value = WaitInputText;
}
