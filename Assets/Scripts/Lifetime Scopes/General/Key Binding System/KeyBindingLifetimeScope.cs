using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class KeyBindingLifetimeScope : LifetimeScope
{
    [SerializeField] private GameObject _bindingsRoot;

    private readonly List<KeyBinderMediator> _keyBinderMediators = new();

    protected override void Configure(IContainerBuilder builder) => 
        builder.Register<KeyListeningTracker>(Lifetime.Singleton);

    private void Start()
    {
        KeyBinderView[] keyBinderViews = _bindingsRoot.GetComponentsInChildren<KeyBinderView>();

        foreach (var keyBinderView in keyBinderViews)
        {
            KeyListeningTracker listeningTracker = Container.Resolve<KeyListeningTracker>();
            InputActions inputActions = Container.Resolve<InputActions>();

            InputAction inputAction = keyBinderView.InputAction;
            KeyBinder keyBinder = null;

            if (inputAction.type == InputActionType.Button)
                keyBinder = new ButtonKeyBinder(listeningTracker, inputActions, inputAction);
            else if (inputAction.type == InputActionType.Value && inputAction.expectedControlType == nameof(Vector2))
                keyBinder = new Vector2KeyBinder(listeningTracker, inputActions, inputAction);

            KeyBinderMediator keyBinderMediator = new(keyBinder, keyBinderView);
            _keyBinderMediators.Add(keyBinderMediator);
        }

        foreach (var keyBinderMediator in _keyBinderMediators)
            keyBinderMediator.Initialize();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        foreach (var keyBinderMediator in _keyBinderMediators)
            keyBinderMediator.Dispose();
    }
}
