using R3;

public class SaveStorageAchievedViewMediator : Mediator
{
    private readonly SaveStorage _saveStorage;
    private readonly AchievedLevelButtonView _achievedLevelButtonView;

    public SaveStorageAchievedViewMediator(SaveStorage saveStorage, 
        AchievedLevelButtonView achievedLevelButtonView)
    {
        _saveStorage = saveStorage;
        _achievedLevelButtonView = achievedLevelButtonView;
    }

    public override void Initialize()
    {
        Observable
            .EveryValueChanged(_achievedLevelButtonView, b => b.isActiveAndEnabled)
            .Where(isActiveAndEnabled => isActiveAndEnabled)
            .Subscribe(_ =>
            {
                bool saveCreated = _saveStorage.Get(SaveConstants.SaveCreatedKey, false);
                _achievedLevelButtonView.SetInteractable(saveCreated);
            })
            .AddTo(CompositeDisposable);
    }
}
