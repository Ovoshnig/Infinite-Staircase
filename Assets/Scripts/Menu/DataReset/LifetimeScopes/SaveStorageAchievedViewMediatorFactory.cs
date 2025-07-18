public class SaveStorageAchievedViewMediatorFactory
    : MediatorViewFactory<SaveStorageAchievedViewMediator, AchievedLevelButtonView>
{
    private readonly SaveStorage _saveStorage;

    public SaveStorageAchievedViewMediatorFactory(SaveStorage saveStorage) =>
        _saveStorage = saveStorage;

    protected override SaveStorageAchievedViewMediator CreateMediatorInstance(AchievedLevelButtonView view) =>
        new(_saveStorage, view);
}
