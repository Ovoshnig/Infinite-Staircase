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
        InputAction viewAction = view.InputAction;
        InputAction foundAction = _inputActions.FindAction(viewAction.id.ToString())
            ?? throw new System.Exception($"Could not find InputAction {viewAction.name} with id {viewAction.id}");

        KeyBinder keyBinder = foundAction.type == InputActionType.Button
            ? new ButtonKeyBinder(_listeningTracker, _settingsStorage, foundAction)
            : new Vector2KeyBinder(_listeningTracker, _settingsStorage, foundAction);
        keyBinder.Initialize();
        Disposables.Add(keyBinder);

        KeyBinderMediator mediator = new(keyBinder, view);
        mediator.Initialize();
        Disposables.Add(mediator);
        return mediator;
    }
}
