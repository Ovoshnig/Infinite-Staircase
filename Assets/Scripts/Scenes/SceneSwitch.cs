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
    private readonly SceneSettings _sceneSettings;
    private readonly ReactiveProperty<bool> _isSceneLoading = new(true);
    private uint _achievedLevel;
    private uint _currentLevel;

    public SceneSwitch(SaveStorage saveStorage, SceneSettings sceneSettings)
    {
        _saveStorage = saveStorage;
        _sceneSettings = sceneSettings;
    }

    public SceneType CurrentSceneType { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsSceneLoading => _isSceneLoading.ToReadOnlyReactiveProperty();

    public void PostInitialize()
    {
        _achievedLevel = _saveStorage.Get(SaveConstants.AchievedLevelKey, _sceneSettings.FirstGameplayLevel);
        _currentLevel = (uint)SceneManager.GetActiveScene().buildIndex;

        if (_currentLevel > _achievedLevel && _currentLevel <= _sceneSettings.LastGameplayLevel)
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

    public void ResetProgress() => _achievedLevel = _sceneSettings.FirstGameplayLevel;

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
        else if (index >= _sceneSettings.FirstGameplayLevel && index <= _sceneSettings.LastGameplayLevel)
            return SceneType.GameLevel;
        else if (index == _sceneSettings.CreditsScene)
            return SceneType.Credits;
        else
            throw new InvalidOperationException($"Invalid level index: {index}");
    }
}
