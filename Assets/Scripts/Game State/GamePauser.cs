using R3;
using UnityEngine;
using Zenject;

public class GamePauser : IInitializable
{
    public ReactiveProperty<bool> IsPause { get; private set; } = new(false);

    public void Initialize() => SetPauseState(pause: false);

    public void Pause()
    {
        //SetPauseState(pause: true);
        IsPause.Value = true;
    }

    public void Unpause()
    {
        //SetPauseState(pause: false);
        IsPause.Value = false;
    }

    private void SetPauseState(bool pause) => Time.timeScale = pause ? 0f : 1f;
}
