using R3;
using UnityEngine;
using Zenject;

public class GamePauser : IInitializable
{
    private readonly ReactiveProperty<bool> _isPause = new(false);

    public ReadOnlyReactiveProperty<bool> IsPause => _isPause;

    public void Initialize() => SetPauseState(pause: false);

    public void Pause()
    {
        _isPause.Value = true;
        SetPauseState(pause: true);
    }

    public void Unpause()
    {
        _isPause.Value = false;
        SetPauseState(pause: false);
    }

    private void SetPauseState(bool pause) => Time.timeScale = pause ? 0f : 1f;
}
