using UnityEngine.InputSystem;

public class KeyBinderMediatorFactory : MediatorViewFactory<KeyBinderMediator, KeyBinderView>
{
    private readonly KeyListeningTracker _listeningTracker;
    private readonly SettingsStorage _settingsStorage;
    private readonly InputActions _inputActions;

    public KeyBinderMediatorFactory(
        KeyListeningTracker listeningTracker, SettingsStorage settingsStorage,
        InputActions inputActions)
    {
        _listeningTracker = listeningTracker;
        _settingsStorage = settingsStorage;
        _inputActions = inputActions;
    }

    public override KeyBinderMediator Create(KeyBinderView view)
    {
        KeyBinder keyBinder = view.InputAction.type == InputActionType.Button
            ? new ButtonKeyBinder(_listeningTracker, _settingsStorage, _inputActions, view.InputAction)
            : new Vector2KeyBinder(_listeningTracker, _settingsStorage, _inputActions, view.InputAction);
        keyBinder.Initialize();
        Disposables.Add(keyBinder);

        KeyBinderMediator mediator = new(keyBinder, view);
        mediator.Initialize();
        Disposables.Add(mediator);
        return mediator;
    }
}
