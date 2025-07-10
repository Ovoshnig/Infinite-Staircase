using UnityEngine.InputSystem;

public class KeyBinderMediatorFactory : MediatorFactory<KeyBinderMediator, KeyBinderView>
{
    private readonly KeyListeningTracker _listeningTracker;
    private readonly InputActions _inputActions;

    public KeyBinderMediatorFactory(
        KeyListeningTracker listeningTracker,
        InputActions inputActions)
    {
        _listeningTracker = listeningTracker;
        _inputActions = inputActions;
    }

    public override KeyBinderMediator Create(KeyBinderView view)
    {
        KeyBinder keyBinder = view.InputAction.type == InputActionType.Button
                ? new ButtonKeyBinder(_listeningTracker, _inputActions, view.InputAction)
                : new Vector2KeyBinder(_listeningTracker, _inputActions, view.InputAction);
        keyBinder.Initialize();

        KeyBinderMediator mediator = new(keyBinder, view);
        mediator.Initialize();
        Disposables.Add(mediator);
        return mediator;
    }
}
