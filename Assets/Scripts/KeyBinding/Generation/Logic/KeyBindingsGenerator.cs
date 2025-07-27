using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(VerticalLayoutGroup))]
public class KeyBindingsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _actionMapTextPrefab;
    [SerializeField] private GameObject _keyBindingBlockPrefab;

    public void GenerateBindings()
    {
        if (!TryDestroyOld())
            return;

        GenerateNew();
    }

    private bool TryDestroyOld()
    {
        List<GameObject> children = transform
            .Cast<Transform>()
            .Select(t => t.gameObject)
            .ToList();

        if (!children.Any())
            return true;

        foreach (var child in children)
        {
            if (child.GetComponent<KeyBinderView>() == null &&
                child.GetComponent<ActionMapTextView>() == null)
            {
                Debug.LogError($"Invalid child object {child.name}, cancelling generation.", child);
                return false;
            }
        }

        foreach (var child in children)
            DestroyImmediate(child, false);

        return true;
    }

    private void GenerateNew()
    {
        InputActionAsset inputActionAsset = new InputActions().asset;

        foreach (var actionMap in inputActionAsset.actionMaps)
        {
            List<InputAction> actionsToBind = new();

            foreach (var action in actionMap.actions)
            {
                List<string> controlPaths = action.controls.Select(c => c.path.ToLower()).ToList();
                bool isKeyboard = controlPaths.Any(p => p.Contains("keyboard"));
                bool hasEscape = controlPaths.Any(p => p.Contains("escape"));

                if (isKeyboard && !hasEscape)
                    actionsToBind.Add(action);
            }

            if (!actionsToBind.Any())
                continue;

            GameObject mapLabel = Instantiate(_actionMapTextPrefab, transform);
            mapLabel.name = $"🗺️{actionMap.name}";
            mapLabel.GetComponent<TMP_Text>().text = actionMap.name;

            foreach (var action in actionsToBind)
            {
                GameObject bindingBlock = Instantiate(_keyBindingBlockPrefab, transform);
                bindingBlock.name = $"🎬{action.name}";

                KeyBinderView keyBinderView = bindingBlock.GetComponent<KeyBinderView>();
                string formattedName = Regex.Replace(action.name, "([A-Z])", " $1").ToLower();
                keyBinderView.SetInputActionReference(InputActionReference.Create(action), formattedName);

                string actionDisplayName = string
                    .Join("/", action.controls.Select(c => c.ToCaseIndependentString()));
                keyBinderView.SetBindingText(actionDisplayName);
            }
        }

        Debug.Log("Generation successfully completed");
    }
}
