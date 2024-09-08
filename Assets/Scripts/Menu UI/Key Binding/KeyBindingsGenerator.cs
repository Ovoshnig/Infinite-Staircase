using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class KeyBindingsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _actionMapTextPrefab;
    [SerializeField] private GameObject _keyBindingBlockPrefab;

    [ContextMenu(nameof(Generate))]
    private void Generate()
    {
        while (transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);

        PlayerInput playerInput = new();
        var actionAsset = playerInput.asset;

        foreach (var actionMap in actionAsset.actionMaps)
        {
            var text = Instantiate(_actionMapTextPrefab, transform).GetComponent<TMP_Text>();
            text.text = actionMap.name;

            foreach (var action in actionMap.actions)
            {
                var keyBinder = Instantiate(_keyBindingBlockPrefab, transform).GetComponent<KeyBinder>();
                var actionName = Regex.Replace(action.name, "([A-Z])", " $1").ToLower();
                keyBinder.ActionNameText.text = actionName;
                keyBinder.InputAction = action;
            }
        }
    }
}
