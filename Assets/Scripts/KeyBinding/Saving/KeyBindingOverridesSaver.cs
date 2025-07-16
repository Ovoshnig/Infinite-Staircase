using R3;
using System;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class KeyBindingOverridesSaver : IInitializable, IDisposable
{
    private readonly SettingsStorage _settingsStorage;
    private readonly InputActions _inputActions;
    private readonly CompositeDisposable _compositeDisposable = new();

    public KeyBindingOverridesSaver(SettingsStorage settingsStorage,
        InputActions inputActions)
    {
        _settingsStorage = settingsStorage;
        _inputActions = inputActions;
    }

    public void Initialize()
    {
        string defaultJson = string.Empty;
        string json = _settingsStorage.Get(SettingsConstants.BindingOverridesKey, defaultJson);
        _inputActions.LoadBindingOverridesFromJson(json);

        _settingsStorage.ResetHappened
            .Subscribe(_ => _inputActions.LoadBindingOverridesFromJson(defaultJson))
            .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        string json = _inputActions.SaveBindingOverridesAsJson();
        _settingsStorage.Set(SettingsConstants.BindingOverridesKey, json);

        _compositeDisposable?.Dispose();
    }
}
