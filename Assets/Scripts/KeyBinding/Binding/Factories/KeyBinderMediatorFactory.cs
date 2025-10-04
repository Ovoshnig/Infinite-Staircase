using R3;
using System;
using UnityEngine.InputSystem;

public class KeyBinderMediatorFactory : MediatorViewFactory<KeyBinderMediator, KeyBinderView>
{
    private readonly ButtonListener _buttonListener;
    private readonly SettingsStorage _settingsStorage;
    private readonly InputActions _inputActions;
    private readonly KeyBindingConflictUpdater _keyBindingConflictChecker;

    public KeyBinderMediatorFactory(ButtonListener buttonListener,
        SettingsStorage settingsStorage,
        InputActions inputActions,
        KeyBindingConflictUpdater keyBindingConflictChecker)
    {
        _buttonListener = buttonListener;
        _settingsStorage = settingsStorage;
        _inputActions = inputActions;
        _keyBindingConflictChecker = keyBindingConflictChecker;
    }

    protected override KeyBinderMediator CreateMediatorInstance(KeyBinderView view)
    {
        InputAction viewAction = view.InputAction;
        InputAction foundAction = _inputActions.FindAction(viewAction.id.ToString())
            ?? throw new InvalidOperationException(
                $"Could not find InputAction {viewAction.name} with id {viewAction.id}");

        KeyBinder keyBinder = (foundAction.type, foundAction.expectedControlType) switch
        {
            (InputActionType.Button, _) =>
                new ButtonKeyBinder(_buttonListener, _settingsStorage, foundAction),
            (InputActionType.Value, var expected) when string.Equals(expected, "Axis", StringComparison.OrdinalIgnoreCase) =>
                new AxisKeyBinder(_buttonListener, _settingsStorage, foundAction),
            (InputActionType.Value, var expected) when string.Equals(expected, "Vector2", StringComparison.OrdinalIgnoreCase) =>
                new Vector2KeyBinder(_buttonListener, _settingsStorage, foundAction),
            (InputActionType.PassThrough, _) =>
                throw new NotSupportedException($"KeyBinder is not provided for {InputActionType.PassThrough}."),
            _ => throw new NotSupportedException($"Action type {foundAction.type} / "
                + $"control {foundAction.expectedControlType} is not supported.")
        };

        keyBinder.Initialize();
        keyBinder.AddTo(CompositeDisposable);

        _keyBindingConflictChecker.AddKeyBinder(foundAction.actionMap, keyBinder);

        return new KeyBinderMediator(keyBinder, view);
    }
}
