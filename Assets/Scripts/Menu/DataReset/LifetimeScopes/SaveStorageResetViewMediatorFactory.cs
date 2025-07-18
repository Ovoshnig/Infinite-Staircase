public class SaveStorageResetViewMediatorFactory
    : MediatorViewFactory<SaveStorageResetViewMediator, SaveResetButtonView>
{
    private readonly SaveStorage _saveStorage;

    public SaveStorageResetViewMediatorFactory(SaveStorage saveStorage) =>
        _saveStorage = saveStorage;

    protected override SaveStorageResetViewMediator CreateMediatorInstance(SaveResetButtonView view) =>
        new(_saveStorage, view);
}
