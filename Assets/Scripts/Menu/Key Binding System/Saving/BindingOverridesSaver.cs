using System;
using UnityEngine.InputSystem;
using Zenject;

public class BindingOverridesSaver : IDisposable
{
    private readonly SettingsStorage _settingsStorage;

    [Inject]
    public BindingOverridesSaver(SettingsStorage settingsStorage)
    {
        _settingsStorage = settingsStorage;

        Initialize();
    }

    public void Initialize()
    {
        var json = _settingsStorage.Get(SettingsConstants.BindingOverridesKey, string.Empty);
        InputSystem.actions.LoadBindingOverridesFromJson(json);
    }

    public void Dispose()
    {
        var json = InputSystem.actions.SaveBindingOverridesAsJson();
        _settingsStorage.Set(SettingsConstants.BindingOverridesKey, json);
    }
}
