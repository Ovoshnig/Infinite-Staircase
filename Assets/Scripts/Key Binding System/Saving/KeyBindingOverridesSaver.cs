using System;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class KeyBindingOverridesSaver : IPostInitializable, IDisposable
{
    private readonly SettingsStorage _settingsStorage;
    private readonly PlayerInput _playerInput;

    public KeyBindingOverridesSaver(SettingsStorage settingsStorage,
        PlayerInput playerInput)
    {
        _settingsStorage = settingsStorage;
        _playerInput = playerInput;
    }

    public void PostInitialize()
    {
        string json = _settingsStorage.Get(SettingsConstants.BindingOverridesKey, string.Empty);
        InputSystem.actions.LoadBindingOverridesFromJson(json);
        _playerInput.LoadBindingOverridesFromJson(json);
    }

    public void Dispose()
    {
        string json = InputSystem.actions.SaveBindingOverridesAsJson();
        _settingsStorage.Set(SettingsConstants.BindingOverridesKey, json);
    }
}
