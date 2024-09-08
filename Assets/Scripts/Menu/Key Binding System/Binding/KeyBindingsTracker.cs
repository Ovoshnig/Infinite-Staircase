using R3;
using System;
using UnityEngine.UI;
using Zenject;

public class KeyBindingsTracker : IInitializable, IDisposable
{
    private readonly ButtonPanelCloser _doneButtonCloser;
    private readonly Button _doneButton;
    private readonly ReactiveProperty<bool> _isListening = new(false);
    private IDisposable _disposable;

    [Inject]
    public KeyBindingsTracker([Inject(Id = ZenjectIdConstants.BindingDoneButtonId)] ButtonPanelCloser buttonPanelCloser)
    {
        _doneButtonCloser = buttonPanelCloser;
        _doneButton = buttonPanelCloser.GetComponent<Button>();
    }

    public void Initialize()
    {
        _disposable = _isListening
            .Subscribe(value =>
            {
                _doneButtonCloser.enabled = !value;
                _doneButton.interactable = !value;
            });
    }

    public void Dispose() => _disposable?.Dispose();

    public bool TryStartListening()
    {
        if (_isListening.Value)
            return false;

        _isListening.Value = true;

        return true;
    }

    public void StopListening() => _isListening.Value = false;
}
