using R3;

public class SaveStorageResetViewMediator : Mediator
{
    private readonly SaveStorage _saveStorage;
    private readonly SaveResetButtonView _saveResetButtonView;

    public SaveStorageResetViewMediator(SaveStorage saveStorage, 
        SaveResetButtonView saveResetButtonView)
    {
        _saveStorage = saveStorage;
        _saveResetButtonView = saveResetButtonView;
    }

    public override void Initialize()
    {
        _saveResetButtonView.Clicked
            .Subscribe(_ => _saveStorage.ResetData())
            .AddTo(CompositeDisposable);
    }
}
