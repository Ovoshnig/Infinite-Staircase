public class SaveStorageAchievedViewMediatorFactory
    : MediatorViewFactory<SaveStorageAchievedViewMediator, AchievedLevelButtonView>
{
    private readonly SaveStorage _saveStorage;

    public SaveStorageAchievedViewMediatorFactory(SaveStorage saveStorage) =>
        _saveStorage = saveStorage;

    public override SaveStorageAchievedViewMediator Create(AchievedLevelButtonView view)
    {
        SaveStorageAchievedViewMediator mediator = new(_saveStorage, view);
        mediator.Initialize();
        Disposables.Add(mediator);
        return mediator;
    }
}
