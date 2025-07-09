using R3;

public class SaveStorageWarningResetButtonViewMediator : Mediator
{
    private readonly SaveStorage _saveStorage;
    private readonly WarningResetButtonView _warningResetButtonView;

    public SaveStorageWarningResetButtonViewMediator(SaveStorage saveStorage, 
        WarningResetButtonView warningResetButtonView)
    {
        _saveStorage = saveStorage;
        _warningResetButtonView = warningResetButtonView;
    }

    public override void Initialize()
    {
        _warningResetButtonView.Clicked
            .Subscribe(_ => _saveStorage.ResetData())
            .AddTo(CompositeDisposable);
    }
}
