using R3;
using UnityEngine;
using VContainer;

public class NewGameButtonView : ButtonView
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _resetWarningPanel;
    [SerializeField] private GameObject _gameCreationPanel;

    private SaveStorage _saveStorage;

    [Inject]
    public void Construct(SaveStorage saveStorage) => 
        _saveStorage = saveStorage;

    protected override void Start()
    {
        base.Start();

        Clicked
            .Subscribe(_ => OnNewGameButtonClicked())
            .AddTo(this);
    }

    private void OnNewGameButtonClicked()
    {
        bool isSaveCreated = _saveStorage.Get(SaveConstants.SaveCreatedKey, false);

        if (isSaveCreated)
            _resetWarningPanel.SetActive(true);
        else
            _gameCreationPanel.SetActive(true);

        _menuPanel.SetActive(false);
    }
}
