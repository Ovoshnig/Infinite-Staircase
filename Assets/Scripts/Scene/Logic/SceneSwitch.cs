using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Threading;
using UnityEngine.SceneManagement;
using VContainer.Unity;

public class SceneSwitch : IInitializable, IDisposable
{
    public enum SceneType
    {
        MainMenu,
        GameLevel,
        Credits
    }

    private readonly SaveStorage _saveStorage;
    private readonly SceneSettings _sceneSettings;
    private readonly ReactiveProperty<bool> _isSceneLoading = new(true);
    private readonly CancellationTokenSource _cts = new();
    private readonly CompositeDisposable _compositeDisposable = new();

    private uint _achievedLevel;
    private uint _currentLevel;

    public SceneSwitch(SaveStorage saveStorage, SceneSettings sceneSettings)
    {
        _saveStorage = saveStorage;
        _sceneSettings = sceneSettings;
    }

    public SceneType CurrentSceneType { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsSceneLoading => _isSceneLoading.ToReadOnlyReactiveProperty();

    public void Initialize()
    {
        _achievedLevel = _saveStorage.Get(SaveConstants.AchievedLevelKey, _sceneSettings.FirstGameplayLevel);

        _saveStorage.ResetHappened
            .Subscribe(_ => ResetAchievedLevel())
            .AddTo(_compositeDisposable);

        _currentLevel = (uint)SceneManager.GetActiveScene().buildIndex;

        if (_currentLevel > _achievedLevel && _currentLevel <= _sceneSettings.LastGameplayLevel)
            _achievedLevel = _currentLevel;

        WaitForFirstSceneLoadAsync().Forget();
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();

        _compositeDisposable.Dispose();

        _saveStorage.Set(SaveConstants.AchievedLevelKey, _achievedLevel);
    }

    public async UniTask LoadAchievedLevelAsync() => await LoadLevelAsync(_achievedLevel);

    public async UniTask LoadCurrentLevelAsync() => await LoadLevelAsync(_currentLevel);

    public async UniTask LoadFirstLevelAsync()
    {
        ResetAchievedLevel();
        await LoadAchievedLevelAsync();
    }

    public async UniTask LoadLevelAsync(uint index)
    {
        try
        {
            SceneType sceneType = GetSceneTypeByIndex(index);
            _isSceneLoading.Value = true;

            await SceneManager.LoadSceneAsync((int)index)
                .ToUniTask(cancellationToken: _cts.Token);

            _currentLevel = index;
            CurrentSceneType = sceneType;
            _isSceneLoading.Value = false;
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    private async UniTask WaitForFirstSceneLoadAsync()
    {
        try
        {
            await UniTask.WaitUntil(() => SceneManager
                .GetActiveScene().isLoaded, cancellationToken: _cts.Token);

            SceneType sceneType = GetSceneTypeByIndex(_currentLevel);
            CurrentSceneType = sceneType;
            _isSceneLoading.Value = false;

            bool saveCreated = _saveStorage.Get(SaveConstants.SaveCreatedKey, false);

            if (sceneType != SceneType.MainMenu && !saveCreated )
                _saveStorage.Set(SaveConstants.SaveCreatedKey, true);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    private void ResetAchievedLevel() => _achievedLevel = _sceneSettings.FirstGameplayLevel;

    private SceneType GetSceneTypeByIndex(uint index)
    {
        if (index == 0)
            return SceneType.MainMenu;
        else if (index >= _sceneSettings.FirstGameplayLevel && index <= _sceneSettings.LastGameplayLevel)
            return SceneType.GameLevel;
        else if (index == _sceneSettings.CreditsScene)
            return SceneType.Credits;
        else
            throw new InvalidOperationException($"Invalid level index: {index}");
    }
}
