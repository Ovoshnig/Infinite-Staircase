using R3;
using System;
using UnityEngine;
using VContainer.Unity;

public class GamePauser : IInitializable, IDisposable
{
    private readonly SceneSwitch _sceneSwitch;
    private readonly TimeSettings _timeSettings;
    private readonly Subject<bool> _isPause = new();
    private readonly CompositeDisposable _compositeDisposable = new();

    public GamePauser(SceneSwitch sceneSwitch, 
        TimeSettings timeSettings)
    {
        _sceneSwitch = sceneSwitch;
        _timeSettings = timeSettings;
    }

    public Observable<bool> IsPause => _isPause;

    public void Initialize()
    {
        _sceneSwitch.IsSceneLoading
            .Where(value => !value)
            .Subscribe(value => Unpause())
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();

    public void Pause() => SetPauseState(true);

    public void Unpause() => SetPauseState(false);

    private void SetPauseState(bool pause)
    {
        _isPause.OnNext(pause);
        Time.timeScale = pause ? _timeSettings.PauseTimeScale : _timeSettings.NormalTimeScale;
    }
}
