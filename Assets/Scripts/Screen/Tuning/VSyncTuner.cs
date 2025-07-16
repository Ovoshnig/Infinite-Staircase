using R3;
using System;
using UnityEngine;
using VContainer.Unity;

public class VSyncTuner : IInitializable, IDisposable
{
    private readonly SettingsStorage _settingsStorage;
    private readonly ReactiveProperty<bool> _isVSyncEnabled = new();
    private readonly CompositeDisposable _compositeDisposable = new();

    public VSyncTuner(SettingsStorage settingsStorage) => _settingsStorage = settingsStorage;

    public ReadOnlyReactiveProperty<bool> IsVSyncEnabled => _isVSyncEnabled;

    public void Initialize()
    {
        bool defaultValue = false;
        _isVSyncEnabled.Value = _settingsStorage.Get(SettingsConstants.VSyncKey, defaultValue);
        QualitySettings.vSyncCount = IsVSyncEnabled.CurrentValue ? 1 : 0;
        Application.targetFrameRate = -1;

        _settingsStorage.ResetHappened
            .Subscribe(_ => DisableVSync())
            .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _settingsStorage.Set(SettingsConstants.VSyncKey, _isVSyncEnabled.Value);

        _compositeDisposable.Dispose();
    }

    public void SwitchVSync()
    {
        if (IsVSyncEnabled.CurrentValue)
            DisableVSync();
        else
            EnableVSync();
    }

    public void EnableVSync()
    {
        QualitySettings.vSyncCount = 1;
        _isVSyncEnabled.Value = true;
    }

    public void DisableVSync()
    {
        QualitySettings.vSyncCount = 0;
        _isVSyncEnabled.Value = false;
    }
}
