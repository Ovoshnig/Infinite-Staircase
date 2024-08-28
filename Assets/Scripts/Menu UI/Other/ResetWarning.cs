using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ResetWarning : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _gameCreationPanel;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    private SaveStorage _saveStorage;

    [Inject]
    private void Construct(SaveStorage saveStorage) => _saveStorage = saveStorage;

    private void OnEnable()
    {
        _yesButton.onClick.AddListener(OnYesButtonClicked);
        _noButton.onClick.AddListener(OnNoButtonClicked);
    }

    private void OnDisable()
    {
        _yesButton.onClick.RemoveListener(OnYesButtonClicked);
        _noButton.onClick.RemoveListener(OnNoButtonClicked);
    }

    private void OnYesButtonClicked() 
    {
        _saveStorage.ResetData();
        _saveStorage.Set(SaveConstants.SaveCreatedKey, false);

        _gameCreationPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnNoButtonClicked()
    {
        _menuPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
