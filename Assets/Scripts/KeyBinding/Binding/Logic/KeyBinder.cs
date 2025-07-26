using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using VContainer.Unity;

public abstract class KeyBinder : IInitializable, IDisposable
{
    private readonly ButtonListener _buttonListener;
    private readonly SettingsStorage _settingsStorage;
    private readonly InputAction _inputAction;
    private readonly ReactiveProperty<bool> _hasOverrides = new(false);
    private readonly ReactiveProperty<bool> _isListening = new(false);
    private readonly ReactiveProperty<bool> _hasConflict = new(false);
    private readonly ReactiveProperty<IReadOnlyList<InputControl>> _controls = new();
    private readonly ReactiveProperty<string> _bindingText = new();
    private readonly CompositeDisposable _settingsDisposable = new();

    private InputControl[] _tempControls;
    private int _inputIndex;

    protected KeyBinder(ButtonListener buttonListener,
        SettingsStorage settingsStorage,
        InputAction inputAction)
    {
        _buttonListener = buttonListener;
        _settingsStorage = settingsStorage;
        _inputAction = inputAction;
    }

    public ReadOnlyReactiveProperty<bool> HasOverrides => _hasOverrides;
    public ReadOnlyReactiveProperty<bool> IsListening => _isListening;
    public ReadOnlyReactiveProperty<bool> HasConflict => _hasConflict;
    public ReadOnlyReactiveProperty<IReadOnlyList<InputControl>> Controls => _controls;
    public ReadOnlyReactiveProperty<string> BindingText => _bindingText;

    protected InputAction InputAction => _inputAction;
    protected abstract int RequiredInputsCount { get; }

    public virtual void Initialize()
    {
        _hasOverrides.Value = _inputAction.bindings.Any(b => b.hasOverrides);
        _controls.Value = _inputAction.controls;
        _bindingText.Value = GetActionDisplayName();

        _settingsStorage.ResetHappened
            .Subscribe(_ => RemoveBindingOverrides())
            .AddTo(_settingsDisposable);
    }

    public virtual void Dispose() => _settingsDisposable.Dispose();

    public virtual void StartListening()
    {
        if (!_buttonListener.TryStartListening(OnAnyButtonPress))
            return;

        _inputIndex = 0;
        _tempControls = new InputControl[RequiredInputsCount];

        _isListening.Value = true;
        _bindingText.Value = GetWaitingText(_inputIndex);
    }

    public virtual void ApplyBindingOverrides()
    {
        ApplyBindingOverrides(_tempControls);
        _hasOverrides.Value = true;
        _controls.Value = _inputAction.controls;
        _bindingText.Value = GetActionDisplayName();
    }

    public virtual void RemoveBindingOverrides()
    {
        _inputAction.RemoveAllBindingOverrides();
        _hasOverrides.Value = false;
        _controls.Value = _inputAction.controls;
        _bindingText.Value = GetActionDisplayName();
    }

    public void SetConflict(bool value) => _hasConflict.Value = value;

    protected virtual string GetActionDisplayName() =>
        string.Join("/", InputAction.controls.Select(c => c.ToCaseIndependentString()));

    protected abstract string GetWaitingText(int inputIndex);

    protected abstract void ApplyBindingOverrides(InputControl[] controls);

    protected virtual void PostProcessTemporaryControls(InputControl[] controls) { }

    protected virtual void HandleOverrides(InputControl[] controls)
    {
        bool hasDupes = controls.Distinct().Count() < controls.Length;

        InputBinding[] nonCompositeBindings = _inputAction.bindings
            .Where(b => !b.isComposite)
            .ToArray();

        bool sameAsDefault = controls
            .Select((c, i) => InputControlPath.Matches(nonCompositeBindings[i].path, c))
            .All(m => m);
        bool sameAsCurrent = controls
            .Select((c, i) => InputControlPath.Matches(nonCompositeBindings[i].effectivePath, c))
            .All(m => m);

        if (!hasDupes && !sameAsCurrent)
        {
            if (sameAsDefault)
                RemoveBindingOverrides();
            else
                ApplyBindingOverrides();
        }
    }

    private void OnAnyButtonPress(InputControl control)
    {
        if (control == Keyboard.current.escapeKey)
        {
            StopListening();
            return;
        }

        _tempControls[_inputIndex++] = control;

        if (_inputIndex >= RequiredInputsCount)
        {
            PostProcessTemporaryControls(_tempControls);
            HandleOverrides(_tempControls);
            StopListening();
        }
        else
        {
            _bindingText.Value = GetWaitingText(_inputIndex);
        }
    }

    private void StopListening()
    {
        _buttonListener.StopListening();

        _isListening.Value = false;
        _bindingText.Value = GetActionDisplayName();
    }
}
