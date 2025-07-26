using R3;
using System;
using UnityEngine.InputSystem;

public class ButtonListener : IDisposable
{
    private readonly ReactiveProperty<bool> _isListening = new(false);
    private readonly CompositeDisposable _listeningDisposable = new();

    public ReadOnlyReactiveProperty<bool> IsListening => _isListening;

    public bool TryStartListening(Action<InputControl> callback)
    {
        if (_isListening.Value)
            return false;

        InputSystem.onAnyButtonPress
            .ToObservable()
            .Where(c => c.device is Keyboard)
            .Subscribe(callback)
            .AddTo(_listeningDisposable);

        _isListening.Value = true;
        return true;
    }

    public void StopListening()
    {
        _listeningDisposable.Clear();
        _isListening.Value = false;
    }

    public void Dispose() => _listeningDisposable.Dispose();
}
