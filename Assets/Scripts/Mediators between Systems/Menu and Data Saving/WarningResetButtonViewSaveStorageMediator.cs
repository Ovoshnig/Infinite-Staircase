using R3;

public class WarningResetButtonViewSaveStorageMediator : Mediator
{
    private readonly WarningResetButtonView _warningResetButtonView;
    private readonly SaveStorage _saveStorage;

    public WarningResetButtonViewSaveStorageMediator(WarningResetButtonView warningResetButtonView, 
        SaveStorage saveStorage)
    {
        _warningResetButtonView = warningResetButtonView;
        _saveStorage = saveStorage;
    }

    public override void Initialize()
    {
        _warningResetButtonView.Clicked
            .Subscribe(_ => _saveStorage.ResetData())
            .AddTo(CompositeDisposable);
    }
}
