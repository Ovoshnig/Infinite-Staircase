using R3;
using System;
using VContainer.Unity;

public class GamePauser : IInitializable, IDisposable
{
    private readonly SceneSwitch _sceneSwitch;
    private readonly ReactiveProperty<bool> _isPause = new();
    private readonly CompositeDisposable _compositeDisposable = new();

    public GamePauser(SceneSwitch sceneSwitch) => _sceneSwitch = sceneSwitch;

    public ReadOnlyReactiveProperty<bool> IsPause => _isPause;

    public void Initialize()
    {
        _sceneSwitch.IsSceneLoading
            .Where(isLoading => !isLoading)
            .Subscribe(_ => UnPause())
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable.Dispose();

    public void Pause() => SetPauseState(true);

    public void UnPause() => SetPauseState(false);

    private void SetPauseState(bool value) => _isPause.OnNext(value);
}
