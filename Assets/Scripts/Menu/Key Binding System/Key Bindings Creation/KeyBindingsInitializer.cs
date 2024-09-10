using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class KeyBindingsInitializer : MonoBehaviour
{
    private PlayerInput _playerInput;

    [Inject]
    private void Construct(PlayerInput playerInput)
    {
        _playerInput = playerInput;

        Initialize();
    }

    private void Initialize()
    {
        var keyBinders = GetComponentsInChildren<KeyBinder>();
        var actionAsset = _playerInput.asset;

        foreach (var actionMap in actionAsset.actionMaps)
        {
            foreach (var action in actionMap.actions)
            {
                try
                {
                    var keyBinder = keyBinders.First(k => k.InputAction.name == action.name);
                    keyBinder.InputAction = action;
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
    }
}
