using Cysharp.Threading.Tasks;
using R3;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

public class SceneSwitch : IPostInitializable, IDisposable
{
    public enum SceneType
    {
        MainMenu,
        GameLevel,
        Credits
    }

    private readonly SaveStorage _saveStorage;
    private readonly LevelSettings _levelSettings;
    private readonly ReactiveProperty<bool> _isSceneLoading = new(true);
    private uint _achievedLevel;
    private uint _currentLevel;

    public SceneSwitch(SaveStorage saveStorage, LevelSettings levelSettings)
    {
        _saveStorage = saveStorage;
        _levelSettings = levelSettings;
    }

    public SceneType CurrentSceneType { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsSceneLoading => _isSceneLoading.ToReadOnlyReactiveProperty();

    public void PostInitialize()
    {
        _achievedLevel = _saveStorage.Get(SaveConstants.AchievedLevelKey, _levelSettings.FirstGameplayLevel);
        _currentLevel = (uint)SceneManager.GetActiveScene().buildIndex;

        if (_currentLevel > _achievedLevel && _currentLevel <= _levelSettings.LastGameplayLevel)
            _achievedLevel = _currentLevel;

        WaitForFirstSceneLoadAsync().Forget();
    }

    public void Dispose() => _saveStorage.Set(SaveConstants.AchievedLevelKey, _achievedLevel);

    public async UniTask LoadAchievedLevelAsync() => await LoadLevelAsync(_achievedLevel);

    public async UniTask LoadFirstLevelAsync()
    {
        ResetProgress();
        await LoadAchievedLevelAsync();
    }

    public void ResetProgress() => _achievedLevel = _levelSettings.FirstGameplayLevel;

    public void LoadCurrentLevel() => LoadLevelAsync(_currentLevel).Forget();

    public async UniTask LoadLevelAsync(uint index)
    {
        SceneType sceneType = GetSceneTypeByIndex(index);
        _isSceneLoading.Value = true;

        await SceneManager.LoadSceneAsync((int)index);

        _currentLevel = index;
        CurrentSceneType = sceneType;
        _isSceneLoading.Value = false;
    }

    private async UniTask WaitForFirstSceneLoadAsync()
    {
        await UniTask.WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        SceneType sceneType = GetSceneTypeByIndex(_currentLevel);
        CurrentSceneType = sceneType;
        _isSceneLoading.Value = false;
    }

    private SceneType GetSceneTypeByIndex(uint index)
    {
        if (index == 0)
            return SceneType.MainMenu;
        else if (index >= _levelSettings.FirstGameplayLevel && index <= _levelSettings.LastGameplayLevel)
            return SceneType.GameLevel;
        else if (index == _levelSettings.CreditsScene)
            return SceneType.Credits;
        else
            throw new InvalidOperationException($"Invalid level index: {index}");
    }
}
