using System;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class KeyBindingOverridesSaver : IPostInitializable, IDisposable
{
    private readonly SettingsStorage _settingsStorage;
    private readonly InputActions _inputActions;

    public KeyBindingOverridesSaver(SettingsStorage settingsStorage,
        InputActions inputActions)
    {
        _settingsStorage = settingsStorage;
        _inputActions = inputActions;
    }

    public void PostInitialize()
    {
        string json = _settingsStorage.Get(SettingsConstants.BindingOverridesKey, string.Empty);
        InputSystem.actions.LoadBindingOverridesFromJson(json);
        _inputActions.LoadBindingOverridesFromJson(json);
    }

    public void Dispose()
    {
        string json = InputSystem.actions.SaveBindingOverridesAsJson();
        _settingsStorage.Set(SettingsConstants.BindingOverridesKey, json);
    }
}
