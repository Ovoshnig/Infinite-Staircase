using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class NewGameButtonView : ButtonView
{
    [SerializeField] private Button _continueGameButton;
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
            .Skip(1)
            .Subscribe(_ => OnNewGameButtonClicked())
            .AddTo(this);

        bool saveCreated = _saveStorage.Get(SaveConstants.SaveCreatedKey, false);
        _continueGameButton.interactable = saveCreated;
    }

    private void OnNewGameButtonClicked()
    {
        if (_saveStorage.Get(SaveConstants.SaveCreatedKey, false))
            _resetWarningPanel.SetActive(true);
        else
            _gameCreationPanel.SetActive(true);

        _menuPanel.SetActive(false);
    }
}
