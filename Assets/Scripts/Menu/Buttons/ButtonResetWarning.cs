using UnityEngine;
using UnityEngine.UI;
using VContainer;

[RequireComponent(typeof(Button))]
public class ButtonResetWarning : MonoBehaviour
{
    private SaveStorage _saveStorage;
    private Button _button;

    [Inject]
    public void Construct(SaveStorage saveStorage) => _saveStorage = saveStorage;

    private void Awake() => _button = GetComponent<Button>();

    private void OnEnable() => _button.onClick.AddListener(OnResetButtonClicked);

    private void OnDisable() => _button.onClick.RemoveListener(OnResetButtonClicked);

    private void OnResetButtonClicked() 
    {
        _saveStorage.ResetData();
        _saveStorage.Set(SaveConstants.SaveCreatedKey, false);
    }
}
