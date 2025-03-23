using System;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class KeyBindingOverridesSaver : IPostInitializable, IDisposable
{
    private readonly SettingsStorage _settingsStorage;

    public KeyBindingOverridesSaver(SettingsStorage settingsStorage) => _settingsStorage = settingsStorage;

    public void PostInitialize()
    {
        string json = _settingsStorage.Get(SettingsConstants.BindingOverridesKey, string.Empty);
        InputSystem.actions.LoadBindingOverridesFromJson(json);
    }

    public void Dispose()
    {
        string json = InputSystem.actions.SaveBindingOverridesAsJson();
        _settingsStorage.Set(SettingsConstants.BindingOverridesKey, json);
    }
}
