using System;
using UnityEngine.InputSystem;
using Zenject;

public class BindingOverridesSaver : IDisposable
{
    private readonly PlayerInput _playerInput;
    private readonly SettingsStorage _settingsStorage;

    [Inject]
    public BindingOverridesSaver(PlayerInput playerInput, SettingsStorage settingsStorage)
    {
        _playerInput = playerInput;
        _settingsStorage = settingsStorage;

        Initialize();
    }

    public void Initialize()
    {
        var json = _settingsStorage.Get(SettingsConstants.BindingOverridesKey, string.Empty);
        _playerInput.LoadBindingOverridesFromJson(json);
    }

    public void Dispose()
    {
        var json = _playerInput.SaveBindingOverridesAsJson();
        _settingsStorage.Set(SettingsConstants.BindingOverridesKey, json);
    }
}