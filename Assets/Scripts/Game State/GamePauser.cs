using System;
using UnityEngine;
using Zenject;

public class GamePauser : IInitializable
{
    public event Action Paused;
    public event Action Unpaused;

    public void Initialize() => SetPauseState(pause: false);

    public void Pause()
    {
        SetPauseState(pause: true);
        Paused?.Invoke();
    }

    public void Unpause()
    {
        SetPauseState(pause: false);
        Unpaused?.Invoke();
    }

    private void SetPauseState(bool pause) => Time.timeScale = pause ? 0f : 1f;
}
