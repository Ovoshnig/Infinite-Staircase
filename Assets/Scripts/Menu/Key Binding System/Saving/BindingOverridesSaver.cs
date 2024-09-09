using System;
using UnityEngine;
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
        Debug.Log(nameof(BindingOverridesSaver));

        var json = _settingsStorage.Get(SettingsConstants.RebindsKey, string.Empty);
        _playerInput.LoadBindingOverridesFromJson(json);
    }

    public void Dispose()
    {
        var json = _playerInput.SaveBindingOverridesAsJson();
        _settingsStorage.Set(SettingsConstants.RebindsKey, json);
    }
}
