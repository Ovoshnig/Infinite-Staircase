using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ScreenTuner : IInitializable, IDisposable
{
    private readonly ScreenInputHandler _inputHandler;
    private readonly ReactiveProperty<bool> _isFullScreen = new(Screen.fullScreen);
    private IDisposable _disposable;

    [Inject]
    public ScreenTuner(ScreenInputHandler screenInputHandler) => _inputHandler = screenInputHandler;

    public List<(int width, int height)> Resolutions { get; private set; }
    public int CurrentResolutionNumber { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsFullScreen => _isFullScreen;

    private (int width, int height) CurrentResolution
    {
        get
        {
            if (_isFullScreen.Value)
                return (Screen.currentResolution.width, Screen.currentResolution.height);
            else
                return (Screen.width, Screen.height);
        }
    }

    public void Initialize()
    {
        var resolution = CurrentResolution;
        var resolutions = Screen.resolutions.Select(x => (x.width, x.height)).ToList();
        Resolutions = resolutions;

        if (resolutions.Contains(resolution))
        {
            CurrentResolutionNumber = resolutions.IndexOf(resolution);
        }
        else
        {
            int index = resolutions.BinarySearch(resolution);

            if (index < 0)
                index = ~index;

            resolutions.Insert(index, resolution);
            CurrentResolutionNumber = index;
        }

        _disposable = _inputHandler.IsSwitchFullScreenPressed
            .Where(value => value)
            .Subscribe(_ => SwitchFullScreen());
    }

    public void Dispose() => _disposable?.Dispose();

    public void SwitchFullScreen()
    {
        _isFullScreen.Value = !_isFullScreen.Value;
        Screen.fullScreen = _isFullScreen.Value;
    }

    public void SetResolution(int number)
    {
        if (number < 0 || number >= Resolutions.Count)
        {
            Debug.LogError($"Resolution with index {number} not found");

            return;
        }

        var (width, height) = Resolutions[number];
        Screen.SetResolution(width, height, Screen.fullScreenMode);
        CurrentResolutionNumber = number;
    }
}
