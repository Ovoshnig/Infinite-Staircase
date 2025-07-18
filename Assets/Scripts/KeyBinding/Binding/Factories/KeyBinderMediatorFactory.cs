using R3;
using UnityEngine.InputSystem;

public class KeyBinderMediatorFactory : MediatorViewFactory<KeyBinderMediator, KeyBinderView>
{
    private readonly KeyListeningTracker _listeningTracker;
    private readonly SettingsStorage _settingsStorage;
    private readonly InputActions _inputActions;

    public KeyBinderMediatorFactory(KeyListeningTracker listeningTracker, SettingsStorage settingsStorage,
        InputActions inputActions)
    {
        _listeningTracker = listeningTracker;
        _settingsStorage = settingsStorage;
        _inputActions = inputActions;
    }

    protected override KeyBinderMediator CreateMediatorInstance(KeyBinderView view)
    {
        InputAction viewAction = view.InputAction;
        InputAction foundAction = _inputActions.FindAction(viewAction.id.ToString())
            ?? throw new System.Exception("Could not find " +
            $"InputAction {viewAction.name} with id {viewAction.id}");

        KeyBinder keyBinder = foundAction.type == InputActionType.Button
            ? new ButtonKeyBinder(_listeningTracker, _settingsStorage, foundAction)
            : new Vector2KeyBinder(_listeningTracker, _settingsStorage, foundAction);
        keyBinder.Initialize();
        keyBinder.AddTo(CompositeDisposable);

        return new KeyBinderMediator(keyBinder, view);
    }
}
