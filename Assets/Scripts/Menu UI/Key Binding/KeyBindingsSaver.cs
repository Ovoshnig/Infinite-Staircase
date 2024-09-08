using System;
using UnityEngine.InputSystem;
using Zenject;

public class KeyBindingsSaver : IInitializable, IDisposable
{
    private readonly PlayerInput _playerInput;
    private readonly SettingsStorage _settingsStorage;

    [Inject]
    public KeyBindingsSaver(PlayerInput playerInput, SettingsStorage settingsStorage)
    {
        _playerInput = playerInput;
        _settingsStorage = settingsStorage;
    }

    public void Initialize()
    {
        var json = _settingsStorage.Get(SettingsConstants.RebindsKey, string.Empty);
        _playerInput.LoadBindingOverridesFromJson(json);
    }

    public void Dispose()
    {
        var json = _playerInput.SaveBindingOverridesAsJson();
        _settingsStorage.Set(SettingsConstants.RebindsKey, json);
    }
}
