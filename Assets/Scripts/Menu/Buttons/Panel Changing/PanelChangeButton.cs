using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class PanelChangeButton : MonoBehaviour
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

    protected virtual void Start()
    {
        _button.OnClickAsObservable()
            .Subscribe(_ => OnButtonClicked())
            .AddTo(this);
    }

    protected void Change()
    {
        _newPanel.SetActive(true);
        _currentPanel.SetActive(false);
    }

    private void OnButtonClicked() => Change();
}
