using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ResetWarning : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _gameCreationPanel;
    [SerializeField] private Button _yesButton;

    private SaveStorage _saveStorage;

    [Inject]
    private void Construct(SaveStorage saveStorage) => _saveStorage = saveStorage;

    private void OnEnable() => _yesButton.onClick.AddListener(OnYesButtonClicked);

    private void OnDisable() => _yesButton.onClick.RemoveListener(OnYesButtonClicked);

    private void OnYesButtonClicked() 
    {
        _saveStorage.ResetData();
        _saveStorage.Set(SaveConstants.SaveCreatedKey, false);

        _gameCreationPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
