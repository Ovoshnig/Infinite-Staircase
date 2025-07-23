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
    private readonly ReactiveProperty<bool> _hasOverrides = new();
    private readonly ReactiveProperty<bool> _isListening = new();
    private readonly ReactiveProperty<bool> _hasConflict = new();
    private readonly Subject<string> _anyButtonPressed = new();
    private readonly CompositeDisposable _settingsDisposable = new();
    private readonly CompositeDisposable _listeningDisposable = new();

    public KeyBinder(KeyListeningTracker listeningTracker, SettingsStorage settingsStorage,
        InputAction inputAction)
    {
        _listeningTracker = listeningTracker;
        _settingsStorage = settingsStorage;
        _inputAction = inputAction;
    }

    public abstract string ActionDisplayName { get; }
    public abstract string WaitInputText { get; }
    public ReactiveProperty<ReadOnlyArray<InputControl>> Controls => _controls;
    public ReadOnlyReactiveProperty<bool> HasOverrides => _hasOverrides;
    public ReadOnlyReactiveProperty<bool> IsListening => _isListening;
    public ReadOnlyReactiveProperty<bool> HasConflict => _hasConflict;
    public Observable<string> AnyButtonPressed => _anyButtonPressed;

    protected KeyListeningTracker ListeningTracker => _listeningTracker;
    protected InputAction InputAction => _inputAction;


    public virtual void Initialize()
    {
        _controls.Value = _inputAction.controls;
        _hasOverrides.Value = _inputAction.bindings.Any(b => b.hasOverrides);
        _isListening.Value = false;

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
        NotifyInputWaiting();
    }

    public virtual void ResetBinding()
    {
        _inputAction.RemoveAllBindingOverrides();

        _controls.Value = _inputAction.controls;
        _hasOverrides.Value = false;
    }

    public void SetConflict(bool value) => _hasConflict.Value = value;

    protected abstract void OnAnyButtonPressed(InputControl control);
        
    protected abstract void ApplyBinding(InputControl inputControl);

    protected virtual void StopListening()
    {
        _listeningDisposable.Clear();
        _listeningTracker.StopListening();

        _isListening.Value = false;
    }

    protected void EnableOverrides()
    {
        _controls.Value = _inputAction.controls;
        _hasOverrides.Value = true;
    }

    protected void NotifyInputWaiting() => _anyButtonPressed.OnNext(WaitInputText);
}
