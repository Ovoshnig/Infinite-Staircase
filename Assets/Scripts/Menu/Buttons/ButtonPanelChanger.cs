using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ButtonPanelChanger : MonoBehaviour
{
    [SerializeField] private GameObject _currentPanel;
    [SerializeField] private GameObject _newPanel;

    private Button _button;

    protected CompositeDisposable CompositeDisposable { get; private set; } = new();

    protected virtual void Awake()
    {
        if (_currentPanel == null)
            _currentPanel = transform.parent.gameObject;

        _button = GetComponent<Button>();
    }

    protected virtual void OnEnable()
    {
        _button.OnClickAsObservable()
            .Subscribe(_ => OnButtonClicked())
            .AddTo(CompositeDisposable);
    }

    protected virtual void OnDisable()
    {
        CompositeDisposable?.Dispose();
        CompositeDisposable = new CompositeDisposable();
    }

    protected void Change()
    {
        _newPanel.SetActive(true);
        _currentPanel.SetActive(false);
    }

    private void OnButtonClicked() => Change();
}
