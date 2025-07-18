using VContainer;
using VContainer.Unity;

public class GameSettingsInstaller : IInstaller
{
    private readonly GameSettings _settings;

    public GameSettingsInstaller(GameSettings settings) => _settings = settings;

    public void Install(IContainerBuilder builder)
    {
        builder.RegisterInstance(_settings.SceneSettings);
        builder.RegisterInstance(_settings.AudioSettings);
        builder.RegisterInstance(_settings.WorldGeneration);
        builder.RegisterInstance(_settings.StaircaseGeneration);
        builder.RegisterInstance(_settings.GlassFloorSettings);
        builder.RegisterInstance(_settings.PlayerSettings);
        builder.RegisterInstance(_settings.KeyBindingSettings);
        builder.RegisterInstance(_settings.InventorySettings);
    }
}
