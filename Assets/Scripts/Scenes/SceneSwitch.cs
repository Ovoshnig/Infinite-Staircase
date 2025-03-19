using Cysharp.Threading.Tasks;
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
    private uint _achievedLevel;
    private uint _currentLevel;

    public event Action<SceneType> SceneLoading;
    public event Action<SceneType> SceneLoaded;

    public SceneSwitch(SaveStorage saveStorage, LevelSettings levelSettings)
    {
        _saveStorage = saveStorage;
        _levelSettings = levelSettings;
    }

    public SceneType CurrentSceneType { get; private set; }

    public void PostInitialize()
    {
        _achievedLevel = _saveStorage.Get(SaveConstants.AchievedLevelKey, _levelSettings.FirstGameplayLevel);
        _currentLevel = (uint)SceneManager.GetActiveScene().buildIndex;

        if (_currentLevel > _achievedLevel && _currentLevel <= _levelSettings.LastGameplayLevel)
            _achievedLevel = _currentLevel;

        WaitForFirstSceneLoad().Forget();
    }

    public void Dispose() => _saveStorage.Set(SaveConstants.AchievedLevelKey, _achievedLevel);

    public async UniTask LoadAchievedLevel() => await LoadLevel(_achievedLevel);

    public async UniTask LoadFirstLevel()
    {
        ResetProgress();
        await LoadAchievedLevel();
    }

    public void ResetProgress() => _achievedLevel = _levelSettings.FirstGameplayLevel;

    public void LoadCurrentLevel() => LoadLevel(_currentLevel).Forget();

    public async UniTask LoadLevel(uint index)
    {
        SceneType sceneType = GetSceneTypeByIndex(index);
        SceneLoading?.Invoke(sceneType);

        await SceneManager.LoadSceneAsync((int)index);

        _currentLevel = index;
        CurrentSceneType = sceneType;
        SceneLoaded?.Invoke(sceneType);
    }

    private async UniTaskVoid WaitForFirstSceneLoad()
    {
        await UniTask.WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        SceneType sceneType = GetSceneTypeByIndex(_currentLevel);
        CurrentSceneType = sceneType;
        SceneLoaded?.Invoke(sceneType);
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
