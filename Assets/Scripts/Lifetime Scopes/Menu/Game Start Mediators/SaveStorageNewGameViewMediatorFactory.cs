public class SaveStorageNewGameViewMediatorFactory
    : MediatorViewFactory<SaveStorageNewGameViewMediator, NewGameButtonView>
{
    private readonly SaveStorage _saveStorage;

    public SaveStorageNewGameViewMediatorFactory(SaveStorage saveStorage) => 
        _saveStorage = saveStorage;

    public override SaveStorageNewGameViewMediator Create(NewGameButtonView view)
    {
        SaveStorageNewGameViewMediator mediator = new(_saveStorage, view);
        mediator.Initialize();
        Disposables.Add(mediator);
        return mediator;
    }
}
