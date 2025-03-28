using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer.Unity;

public class ScreenTuner : IInitializable, IDisposable
{
    private readonly ScreenInputHandler _screenInputHandler;
    private readonly ReactiveProperty<bool> _isFullScreen = new(Screen.fullScreen);
    private readonly CompositeDisposable _compositeDisposable = new();

    public ScreenTuner(ScreenInputHandler screenInputHandler) => _screenInputHandler = screenInputHandler;

    public List<(int width, int height, RefreshRate refreshRate)> Resolutions { get; private set; }
    public int CurrentResolutionNumber { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsFullScreen => _isFullScreen;

    private (int width, int height, RefreshRate refreshRate) CurrentResolution
    {
        get
        {
            if (_isFullScreen.Value)
                return (Screen.currentResolution.width, Screen.currentResolution.height, Screen.currentResolution.refreshRateRatio);
            else
                return (Screen.width, Screen.height, Screen.currentResolution.refreshRateRatio);
        }
    }

    public void Initialize()
    {
        var resolution = CurrentResolution;
        var resolutions = Screen.resolutions.Select(x => (x.width, x.height, x.refreshRateRatio)).ToList();
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

        _screenInputHandler.IsSwitchFullScreenPressed
            .Where(value => value)
            .Subscribe(_ => OnSwitchFullScreenPressed())
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();

    public void OnSwitchFullScreenPressed()
    {
        _isFullScreen.Value = !_isFullScreen.Value;
        Screen.fullScreen = _isFullScreen.Value;
        Debug.Log(_isFullScreen.Value);
    }

    public void SetResolution(int number)
    {
        if (number < 0 || number >= Resolutions.Count)
        {
            Debug.LogError($"Resolution with index {number} not found");

            return;
        }

        var (width, height, refreshRate) = Resolutions[number];
        Screen.SetResolution(width, height, Screen.fullScreenMode, refreshRate);
        CurrentResolutionNumber = number;
    }
}
