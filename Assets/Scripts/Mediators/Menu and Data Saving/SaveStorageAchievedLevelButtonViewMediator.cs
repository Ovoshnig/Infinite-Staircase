using R3;
using System;
using VContainer.Unity;

public class SaveStorageAchievedLevelButtonViewMediator : IInitializable, IDisposable
{
    private readonly SaveStorage _saveStorage;
    private readonly AchievedLevelButtonView _achievedLevelButtonView;
    private readonly CompositeDisposable _compositeDisposable = new();

    public SaveStorageAchievedLevelButtonViewMediator(SaveStorage saveStorage, 
        AchievedLevelButtonView achievedLevelButtonView)
    {
        _saveStorage = saveStorage;
        _achievedLevelButtonView = achievedLevelButtonView;
    }

    public void Initialize()
    {
        Observable
            .EveryValueChanged(_achievedLevelButtonView, b => b.isActiveAndEnabled)
            .Where(value => value)
            .Subscribe(_ =>
            {
                bool saveCreated = _saveStorage.Get(SaveConstants.SaveCreatedKey, false);
                _achievedLevelButtonView.SetInteractable(saveCreated);
            })
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
