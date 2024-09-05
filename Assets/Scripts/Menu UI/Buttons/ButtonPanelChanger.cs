using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ButtonPanelChanger : MonoBehaviour
{
    [SerializeField] private GameObject _currentPanel;
    [SerializeField] private GameObject _newPanel;

    private Button _button;

    protected virtual void Awake()
    {
        if (_currentPanel == null)
            _currentPanel = transform.parent.gameObject;

        _button = GetComponent<Button>();
    }

    protected virtual void OnEnable() => _button.onClick.AddListener(OnButtonClicked);

    protected virtual void OnDisable() => _button.onClick.RemoveListener(OnButtonClicked);

    protected void Change()
    {
        _newPanel.SetActive(true);
        _currentPanel.SetActive(false);
    }

    private void OnButtonClicked() => Change();
}
