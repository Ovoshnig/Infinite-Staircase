public class SaveStorageResetViewMediatorFactory
    : MediatorViewFactory<SaveStorageResetViewMediator, SaveResetButtonView>
{
    private readonly SaveStorage _saveStorage;

    public SaveStorageResetViewMediatorFactory(SaveStorage saveStorage) => 
        _saveStorage = saveStorage;

    public override SaveStorageResetViewMediator Create(SaveResetButtonView view)
    {
        SaveStorageResetViewMediator mediator = new(_saveStorage, view);
        mediator.Initialize();
        Disposables.Add(mediator);
        return mediator;
    }
}
