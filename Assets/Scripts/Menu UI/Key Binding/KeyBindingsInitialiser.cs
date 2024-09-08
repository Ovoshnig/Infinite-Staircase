using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class KeyBindingsInitialiser : MonoBehaviour
{
    private PlayerInput _playerInput;

    [Inject]
    private void Construct(PlayerInput playerInput) => _playerInput = playerInput;

    private void Start()
    {
        KeyBinder[] keyBinders = GetComponentsInChildren<KeyBinder>();

        var actionAsset = _playerInput.asset;

        foreach (var actionMap in actionAsset.actionMaps)
        {
            foreach (var action in actionMap.actions)
            {
                var keyBinder = keyBinders.First(k => k.InputAction.name == action.name);
                keyBinder.InputAction = action;
                keyBinder.BindingButtonText.text = action.GetBindingDisplayString();
            }
        }
    }
}
