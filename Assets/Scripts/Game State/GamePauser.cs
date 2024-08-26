using R3;
using System;
using UnityEngine;
using Zenject;

public class GamePauser : IInitializable, IDisposable
{
    private readonly SceneSwitch _sceneSwitch;
    private readonly ReactiveProperty<bool> _isPause = new(false);

    public GamePauser(SceneSwitch sceneSwitch) => _sceneSwitch = sceneSwitch;

    public ReadOnlyReactiveProperty<bool> IsPause => _isPause;

    public void Initialize() => _sceneSwitch.SceneLoaded += OnSceneLoaded;

    public void Dispose() => _sceneSwitch.SceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(SceneSwitch.SceneType type) => Unpause();

    public void Pause() => SetPauseState(true);

    public void Unpause() => SetPauseState(false);

    private void SetPauseState(bool pause)
    {
        _isPause.Value = pause;
        Time.timeScale = pause ? 0f : 1f;
    }
}
