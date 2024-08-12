using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameSettingsLifetimeScope : LifetimeScope
{
    [SerializeField] private GameSettings _gameSettings;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_gameSettings.ControlSettings);
        builder.RegisterInstance(_gameSettings.LevelSettings);
        builder.RegisterInstance(_gameSettings.AudioSettings);
        builder.RegisterInstance(_gameSettings.InventorySettings);
    }
}
