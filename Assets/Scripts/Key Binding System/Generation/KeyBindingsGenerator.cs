using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyBindingsGenerator : MonoBehaviour
{
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private GameObject _actionMapTextPrefab;
    [SerializeField] private GameObject _keyBindingBlockPrefab;

    [ContextMenu(nameof(Generate))]
    private void Generate()
    {
        if (TryDestroyOld())
            GenerateNew();
    }

    private bool TryDestroyOld()
    {
        List<GameObject> directChildren = _parentTransform
                    .Cast<Transform>()
                    .Select(t => t.gameObject)
                    .ToList();

        if (directChildren.Any())
        {
            foreach (var directChild in directChildren)
            {
                if (directChild.GetComponent<KeyBinderView>() == null &&
                    directChild.GetComponent<ActionMapTextView>() == null)
                {
                    Debug.LogError($"Invalid child object {directChild.name}, destroying and " +
                        "generation of key bindings cancelled", directChild);

                    return false;
                }
            }

            foreach (var directChild in directChildren)
                DestroyImmediate(directChild, false);
        }

        return true;
    }

    private void GenerateNew()
    {
        InputActionAsset actionAsset = InputSystem.actions;

        foreach (var actionMap in actionAsset.actionMaps)
        {
            List<InputAction> appropriateActions = new();

            foreach (var action in actionMap.actions)
            {
                List<string> controlPaths = action.controls
                    .Select(c => c.path.ToLower())
                    .ToList();
                bool isKeyboardUsing = controlPaths.Any(c => c.Contains("keyboard"));
                bool hasEscapeKey = controlPaths.Any(c => c.Contains("escape"));

                if (isKeyboardUsing && !hasEscapeKey)
                    appropriateActions.Add(action);
            }

            if (!appropriateActions.Any())
                continue;

            GameObject actionMapObject = Instantiate(_actionMapTextPrefab, _parentTransform);
            actionMapObject.name = $"🗺️{actionMap.name}";

            TMP_Text actionMapText = actionMapObject.GetComponent<TMP_Text>();
            actionMapText.text = actionMap.name;

            foreach (var action in appropriateActions)
            {
                GameObject keyBindingBlockObject = Instantiate(_keyBindingBlockPrefab, _parentTransform);
                keyBindingBlockObject.name = $"🎬{action.name}";

                KeyBinderView keyBinderView = keyBindingBlockObject.GetComponent<KeyBinderView>();
                string actionName = Regex.Replace(action.name, "([A-Z])", " $1").ToLower();
                keyBinderView.SetInputAction(InputActionReference.Create(action), actionName);
            }
        }

        Debug.Log("Generation successfully completed");
    }
}
