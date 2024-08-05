using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneSwitch : IInitializable, IDisposable
{
    public enum SceneType
    {
        MainMenu,
        GameLevel,
        Credits
    }

    private const string AchievedLevelKey = "AchievedLevel";

    private readonly SaveSaver _saveSaver;
    private readonly GameSettingsInstaller.LevelSettings _levelSettings;
    private uint _achievedLevel;
    private uint _currentLevel;
    private bool _isLevelLoading = false;

    public event Action<SceneType> SceneLoading;
    public event Action<SceneType> SceneLoaded;

    [Inject]
    public SceneSwitch(SaveSaver saveSaver, GameSettingsInstaller.LevelSettings levelSettings)
    {
        _saveSaver = saveSaver;
        _levelSettings = levelSettings;
    }

    public SceneType CurrentSceneType { get; private set; }

    public void Initialize()
    {
        _achievedLevel = _saveSaver.LoadData(AchievedLevelKey, _levelSettings.FirstGameplayLevel);
        _currentLevel = (uint)SceneManager.GetActiveScene().buildIndex;

        if (_currentLevel > _achievedLevel && _currentLevel <= _levelSettings.LastGameplayLevel)
            _achievedLevel = _currentLevel;

        WaitForFirstSceneLoad().Forget();
    }

    public void Dispose() => _saveSaver.SaveData(AchievedLevelKey, _achievedLevel);

    public async UniTask LoadAchievedLevel() => await LoadLevel(_achievedLevel);

    public async UniTask LoadFirstLevel()
    {
        ResetProgress();
        await LoadAchievedLevel();
    }

    public void ResetProgress() => _achievedLevel = _levelSettings.FirstGameplayLevel;

    public async UniTask LoadNextLevel()
    {
        if (_currentLevel < _achievedLevel)
            await LoadLevel(_currentLevel + 1);
    }

    public async UniTask<bool> TryLoadNextLevelFirstTime()
    {
        if (_isLevelLoading)
            return false;

        bool isAchievedNextLevel = _currentLevel == _achievedLevel;

        if (isAchievedNextLevel)
        {
            if (_achievedLevel < _levelSettings.LastGameplayLevel)
                _achievedLevel++;

            await LoadLevel(_currentLevel + 1);
        }
        else
        {
            LoadNextLevel().Forget();
        }

        return isAchievedNextLevel;
    }

    public async UniTask LoadPreviousLevel()
    {
        if (_currentLevel > _levelSettings.FirstGameplayLevel)
            await LoadLevel(_currentLevel - 1);
    }

    public void LoadCurrentLevel() => LoadLevel(_currentLevel).Forget();

    public async UniTask LoadLevel(uint index)
    {
        SceneType sceneType = GetSceneTypeByIndex(index);

        _isLevelLoading = true;
        SceneLoading?.Invoke(sceneType);

        await SceneManager.LoadSceneAsync((int)index);

        _isLevelLoading = false;
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
            throw new InvalidOperationException("Invalid level index");
    }
}
