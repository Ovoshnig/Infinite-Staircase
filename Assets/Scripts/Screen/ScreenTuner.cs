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

    public int CurrentResolutionNumber { get; private set; } = 0;
    public ReadOnlyReactiveProperty<bool> IsFullScreen => _isFullScreen;

    private (int width, int height) CurrentResolution
    {
        get
        {
            var resolution = Screen.currentResolution;

            return (resolution.width, resolution.height);
        }
    }

    public void Initialize()
    {
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

    public List<(int width, int height)> GetResolutions()
    {
        var resolution = CurrentResolution;
        var resolutions = Screen.resolutions.Select(x => (x.width, x.height)).ToList();

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

        return resolutions;
    }

    public void SetResolution(int number)
    {
        var resolutions = Screen.resolutions;

        if (number < 0 || number >= resolutions.Length)
        {
            Debug.LogError($"Resolution with index {number} not found");

            return;
        }

        var resolution = resolutions[number];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }
}
