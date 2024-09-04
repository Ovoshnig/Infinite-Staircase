using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class ScreenTuner : IInitializable, IDisposable
{
    private readonly SettingsStorage _settingsStorage;
    private readonly PlayerInput _playerInput = new();

    [Inject]
    public ScreenTuner(SettingsStorage settingsStorage) => _settingsStorage = settingsStorage;

    public int CurrentResolutionNumber { get; private set; } = 0;
    public bool IsFullScreen => Screen.fullScreen;

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
        var (width, height) = _settingsStorage.Get(SettingsConstants.ScreenResolutionKey, CurrentResolution);
        
        Screen.SetResolution(width, height, Screen.fullScreen);

        _playerInput.Screen.SwitchFullscreen.performed += OnSwitchFullScreenPerformed;
        _playerInput.Enable();
    }

    public void Dispose()
    {
        _settingsStorage.Set(SettingsConstants.ScreenResolutionKey, CurrentResolution);

        _playerInput.Disable();
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

    public void SwitchFullScreen() => Screen.fullScreen = !Screen.fullScreen;

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

    private void OnSwitchFullScreenPerformed(InputAction.CallbackContext _) => SwitchFullScreen();
}
