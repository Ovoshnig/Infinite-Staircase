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
        //SetPauseState(pause: true);
        _isPause.Value = true;
    }

    public void Unpause()
    {
        //SetPauseState(pause: false);
        _isPause.Value = false;
    }

    private void SetPauseState(bool pause) => Time.timeScale = pause ? 0f : 1f;
}
