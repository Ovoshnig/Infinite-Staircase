using System;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class KeyBindingOverridesSaver : IInitializable, IDisposable
{
    private readonly SettingsStorage _settingsStorage;

    public KeyBindingOverridesSaver(SettingsStorage settingsStorage) => _settingsStorage = settingsStorage;

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
