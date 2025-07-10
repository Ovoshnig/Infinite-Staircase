using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class KeyBinderMediatorFactory : IDisposable
{
    private readonly KeyListeningTracker _listeningTracker;
    private readonly InputActions _inputActions;
    private readonly List<IDisposable> _disposables = new();

    public KeyBinderMediatorFactory(
        KeyListeningTracker listeningTracker,
        InputActions inputActions)
    {
        _listeningTracker = listeningTracker;
        _inputActions = inputActions;
    }

    public void Dispose()
    {
        foreach (var disposable in _disposables)
            disposable.Dispose();
    }

    public KeyBinderMediator Create(KeyBinderView view)
    {
        KeyBinder binder = view.InputAction.type == InputActionType.Button
                ? new ButtonKeyBinder(_listeningTracker, _inputActions, view.InputAction)
                : new Vector2KeyBinder(_listeningTracker, _inputActions, view.InputAction);
        binder.Initialize();

        KeyBinderMediator mediator = new(binder, view);
        mediator.Initialize();
        _disposables.Add(mediator);
        return mediator;
    }
}
