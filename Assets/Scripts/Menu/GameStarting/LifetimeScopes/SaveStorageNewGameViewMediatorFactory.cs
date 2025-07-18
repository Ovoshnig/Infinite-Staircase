public class SaveStorageNewGameViewMediatorFactory
    : MediatorViewFactory<SaveStorageNewGameViewMediator, NewGameButtonView>
{
    private readonly SaveStorage _saveStorage;

    public SaveStorageNewGameViewMediatorFactory(SaveStorage saveStorage) =>
        _saveStorage = saveStorage;

    protected override SaveStorageNewGameViewMediator CreateMediatorInstance(NewGameButtonView view) =>
        new(_saveStorage, view);
}
