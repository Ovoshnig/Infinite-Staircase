using R3;

public class SaveStorageNewGameViewMediator : Mediator
{
    private readonly SaveStorage _saveStorage;
    private readonly NewGameButtonView _newGameButtonView;

    public SaveStorageNewGameViewMediator(SaveStorage saveStorage, 
        NewGameButtonView newGameButtonView)
    {
        _saveStorage = saveStorage;
        _newGameButtonView = newGameButtonView;
    }

    public override void Initialize()
    {
        _newGameButtonView.Clicked
            .Subscribe(_ => OnButtonClicked())
            .AddTo(CompositeDisposable);
    }

    private void OnButtonClicked()
    {
        bool isSaveCreated = _saveStorage.Get(SaveConstants.SaveCreatedKey, false);

        if (isSaveCreated)
            _newGameButtonView.SetActiveResetWarningPanel(true);
        else
            _newGameButtonView.SetActiveGameCreationPanel(true);

        _newGameButtonView.SetActiveMenuPanel(false);
    }
}
