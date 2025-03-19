using R3;
using System;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer.Unity;
using System.Linq;

public class KeyBindingsTracker : IInitializable, IDisposable
{
    private readonly ButtonPanelCloser _doneButtonCloser;
    private readonly Button _doneButton;
    private readonly ReactiveProperty<bool> _isListening = new(false);
    private readonly CompositeDisposable _compositeDisposable = new();

    public KeyBindingsTracker(ButtonPanelCloser buttonPanelCloser)
    {
        _doneButtonCloser = buttonPanelCloser;
        _doneButton = buttonPanelCloser.GetComponent<Button>();
    }

    public ReadOnlyArray<InputControl> AllControls { get; } =
        Keyboard.current.allControls
        .Where(c => c != Keyboard.current.anyKey)
        .ToArray();

    public void Initialize()
    {
        _isListening
            .Subscribe(value =>
            {
                _doneButtonCloser.enabled = !value;
                _doneButton.interactable = !value;
            })
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();

    public bool TryStartListening()
    {
        if (_isListening.Value)
            return false;

        _isListening.Value = true;

        return true;
    }

    public void StopListening() => _isListening.Value = false;
}
