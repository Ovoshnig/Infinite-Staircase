using R3;
using System;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using VContainer.Unity;

public abstract class KeyBinder : IKeyBinder, IInitializable, IDisposable
{
    private readonly KeyListeningTracker _listeningTracker;
    private readonly SettingsStorage _settingsStorage;
    private readonly InputAction _inputAction;
    private readonly ReactiveProperty<ReadOnlyArray<InputControl>> _controls = new();
    private readonly ReactiveProperty<bool> _hasOverrides = new(false);
    private readonly ReactiveProperty<string> _bindingText = new("Key");
    private readonly ReactiveProperty<bool> _isListening = new(false);
    private readonly ReactiveProperty<bool> _hasConflict = new(false);
    private readonly CompositeDisposable _settingsDisposable = new();
    private readonly CompositeDisposable _listeningDisposable = new();

    public KeyBinder(KeyListeningTracker listeningTracker, SettingsStorage settingsStorage,
        InputAction inputAction)
    {
        _listeningTracker = listeningTracker;
        _settingsStorage = settingsStorage;
        _inputAction = inputAction;
    }

    public ReactiveProperty<ReadOnlyArray<InputControl>> Controls => _controls;
    public ReadOnlyReactiveProperty<bool> HasOverrides => _hasOverrides;
    public ReadOnlyReactiveProperty<string> BindingText => _bindingText;
    public ReadOnlyReactiveProperty<bool> IsListening => _isListening;
    public ReadOnlyReactiveProperty<bool> HasConflict => _hasConflict;

    protected KeyListeningTracker ListeningTracker => _listeningTracker;
    protected InputAction InputAction => _inputAction;
    protected abstract string WaitInputText { get; }

    public virtual void Initialize()
    {
        _controls.Value = _inputAction.controls;
        _hasOverrides.Value = _inputAction.bindings.Any(b => b.hasOverrides);
        _bindingText.Value = GetActionDisplayName();

        _settingsStorage.ResetHappened
            .Subscribe(_ => ResetBinding())
            .AddTo(_settingsDisposable);
    }

    public virtual void Dispose()
    {
        _settingsDisposable.Dispose();
        _listeningDisposable.Dispose();
    }

    public virtual void StartListening()
    {
        InputSystem.onAnyButtonPress
            .ToObservable()
            .Where(control => control.device is Keyboard)
            .Subscribe(OnAnyButtonPressed)
            .AddTo(_listeningDisposable);

        _isListening.Value = true;
        SetWaitingMessage();
    }

    public virtual void ResetBinding()
    {
        _inputAction.RemoveAllBindingOverrides();

        _controls.Value = _inputAction.controls;
        _hasOverrides.Value = false;
        _bindingText.Value = GetActionDisplayName();
    }

    public abstract string GetActionDisplayName();

    public void EnableOverrides()
    {
        _controls.Value = _inputAction.controls;
        _hasOverrides.Value = true;
    }

    public void SetConflict(bool value) => _hasConflict.Value = value;

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
        _listeningDisposable.Clear();
        _listeningTracker.StopListening();
        _isListening.Value = false;
    }

    protected void SetWaitingMessage() => _bindingText.Value = WaitInputText;
}
