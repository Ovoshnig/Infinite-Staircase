using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ResetWarning : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _gameCreationPanel;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    private SaveSaver _saveSaver;

    [Inject]
    private void Construct(SaveSaver saveSaver) => _saveSaver = saveSaver;

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
        _saveSaver.Reset();

        _gameCreationPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnNoButtonClicked()
    {
        _menuPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
