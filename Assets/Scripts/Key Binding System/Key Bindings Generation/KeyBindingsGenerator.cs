using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyBindingsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _actionMapTextPrefab;
    [SerializeField] private GameObject _keyBindingBlockPrefab;

    [ContextMenu(nameof(Generate))]
    private void Generate()
    {
        while (transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);

        var actionAsset = InputSystem.actions;

        foreach (var actionMap in actionAsset.actionMaps)
        {
            var text = Instantiate(_actionMapTextPrefab, transform).GetComponent<TMP_Text>();
            text.text = actionMap.name;

            foreach (var action in actionMap.actions)
            {
                bool isKeyboardUsing = action.controls.Any(c => c.path.ToLower().Contains("keyboard"));
                bool hasEscapeKey = action.controls.Any(c => c.path.ToLower().Contains("escape"));

                if (!isKeyboardUsing || hasEscapeKey)
                    continue;

                var keyBinder = Instantiate(_keyBindingBlockPrefab, transform).GetComponent<KeyBinderView>();
                var actionName = Regex.Replace(action.name, "([A-Z])", " $1").ToLower();
                keyBinder.ActionNameText.text = actionName;
                keyBinder.SetInputActionReference(InputActionReference.Create(action));
            }
        }
    }
}
