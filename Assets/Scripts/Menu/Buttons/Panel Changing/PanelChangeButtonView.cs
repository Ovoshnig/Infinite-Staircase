using R3;
using UnityEngine;

public abstract class PanelChangeButtonView : ButtonView
{
    [SerializeField] private GameObject _currentPanel;
    [SerializeField] private GameObject _newPanel;

    protected override void Awake()
    {
        base.Awake();

        if (_currentPanel == null)
            _currentPanel = transform.parent.gameObject;
    }

    protected override void Start()
    {
        base.Start();

        Clicked
            .Skip(1)
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
