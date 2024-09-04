using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonPanelChanger : MonoBehaviour
{
    [SerializeField] private GameObject _currentPanel;
    [SerializeField] private GameObject _newPanel;

    private Button _button;

    private void Awake() => _button = GetComponent<Button>();

    private void OnEnable() => _button.onClick.AddListener(OnButtonClicked);

    private void OnDisable() => _button.onClick.RemoveListener(OnButtonClicked);

    private void OnButtonClicked()
    {
        _newPanel.SetActive(true);
        _currentPanel.SetActive(false);
    }
}
