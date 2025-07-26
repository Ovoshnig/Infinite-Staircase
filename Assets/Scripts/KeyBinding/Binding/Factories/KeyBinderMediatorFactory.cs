using R3;
using System;
using UnityEngine.InputSystem;

public class KeyBinderMediatorFactory : MediatorViewFactory<KeyBinderMediator, KeyBinderView>
{
    private readonly KeyListeningTracker _listeningTracker;
    private readonly SettingsStorage _settingsStorage;
    private readonly InputActions _inputActions;
    private readonly KeyBindingConflictUpdater _keyBindingConflictChecker;

    public KeyBinderMediatorFactory(KeyListeningTracker listeningTracker, SettingsStorage settingsStorage,
        InputActions inputActions, KeyBindingConflictUpdater keyBindingConflictChecker)
    {
        _listeningTracker = listeningTracker;
        _settingsStorage = settingsStorage;
        _inputActions = inputActions;
        _keyBindingConflictChecker = keyBindingConflictChecker;
    }

    protected override KeyBinderMediator CreateMediatorInstance(KeyBinderView view)
    {
        InputAction viewAction = view.InputAction;
        InputAction foundAction = _inputActions.FindAction(viewAction.id.ToString())
            ?? throw new Exception("Could not find " +
            $"InputAction {viewAction.name} with id {viewAction.id}");

        KeyBinder keyBinder = null;

        switch (foundAction.type)
        {
            case InputActionType.Button:
                keyBinder = new ButtonKeyBinder(_listeningTracker, _settingsStorage, foundAction);
                break;
            case InputActionType.Value:
                keyBinder = foundAction.expectedControlType switch
                {
                    "Axis" => new AxisKeyBinder(_listeningTracker, _settingsStorage, foundAction),
                    "Vector2" => new Vector2KeyBinder(_listeningTracker, _settingsStorage, foundAction),
                    _ => throw new Exception("KeyBinder is not provided for the " +
                        $"{foundAction.expectedControlType} control type."),
                };
                break;
            case InputActionType.PassThrough:
                throw new Exception($"KeyBinder is not provided for the {InputActionType.PassThrough}.");
        }

        _keyBindingConflictChecker.AddKeyBinder(foundAction.actionMap, keyBinder);
        
        keyBinder.Initialize();
        keyBinder.AddTo(CompositeDisposable);

        return new KeyBinderMediator(keyBinder, view);
    }
}
