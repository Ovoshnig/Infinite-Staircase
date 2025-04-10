using R3;
using UnityEngine.InputSystem;

public abstract class BindingHandler : IBindingHandler
{
    private readonly KeyListeningTracker _listeningTracker;
    private readonly InputActions _inputActions;
    private readonly InputAction _inputAction;
    private readonly ReactiveProperty<bool> _isListening = new(false);
    private readonly ReactiveProperty<string> _bindingText = new(string.Empty);
    private CompositeDisposable _compositeDisposable = new();

    public BindingHandler(KeyListeningTracker listeningTracker, 
        InputActions IiputActions, InputAction inputAction)
    {
        _listeningTracker = listeningTracker;
        _inputActions = IiputActions;
        _inputAction = inputAction;
    }

    public ReadOnlyReactiveProperty<bool> IsListening => _isListening;
    public ReadOnlyReactiveProperty<string> BindingText => _bindingText;

    protected KeyListeningTracker ListeningTracker => _listeningTracker;
    protected InputActions InputActionsProperty => _inputActions;
    protected InputAction InputActionProperty => _inputAction;
    protected abstract string WaitInputText { get; }

    public void Initialize() => _bindingText.Value = GetActionDisplayName();

    public void Dispose() { }

    public virtual void StartListening()
    {
        InputSystem.onAnyButtonPress
            .ToObservable()
            .Subscribe(OnAnyButtonPressed)
            .AddTo(_compositeDisposable);

        _isListening.Value = true;
        SetWaitingMessage();
    }

    public virtual void ResetBinding()
    {
        _inputAction.RemoveAllBindingOverrides();
        _inputActions.RemoveAllBindingOverrides();
        _bindingText.Value = GetActionDisplayName();
    }

    public abstract string GetActionDisplayName();

    protected abstract void OnAnyButtonPressed(InputControl control);

    protected virtual void ApplyBinding(InputControl _)
    {
        _bindingText.Value = GetActionDisplayName();

        StopListening();
    }

    protected virtual void CancelListening()
    {
        _bindingText.Value = GetActionDisplayName();

        StopListening();
    }

    protected virtual void StopListening()
    {
        _compositeDisposable?.Dispose();
        _compositeDisposable = new CompositeDisposable();

        _listeningTracker.StopListening();
        _isListening.Value = false;
    }

    protected void SetWaitingMessage() => _bindingText.Value = WaitInputText;
}
