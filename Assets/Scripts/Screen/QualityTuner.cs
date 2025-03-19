using System;
using UnityEngine;
using VContainer.Unity;

public class QualityTuner : IInitializable, IDisposable
{
    private readonly SettingsStorage _settingsStorage;
    private bool _isVSyncEnabled;

    public QualityTuner(SettingsStorage settingsStorage) => _settingsStorage = settingsStorage;

    public bool IsVSyncEnabled => _isVSyncEnabled;

    public void Initialize()
    {
        _isVSyncEnabled = _settingsStorage.Get(SettingsConstants.VSyncKey, false);
        QualitySettings.vSyncCount = _isVSyncEnabled ? 1 : 0;
        Application.targetFrameRate = -1;
    }

    public void Dispose() => _settingsStorage.Set(SettingsConstants.VSyncKey, _isVSyncEnabled);

    public void SwitchVSync()
    {
        if (_isVSyncEnabled)
            DisableVSync();
        else
            EnableVSync();
    }

    private void EnableVSync()
    {
        QualitySettings.vSyncCount = 1;
        _isVSyncEnabled = true;
    }

    private void DisableVSync()
    {
        QualitySettings.vSyncCount = 0;
        _isVSyncEnabled = false;
    }
}
