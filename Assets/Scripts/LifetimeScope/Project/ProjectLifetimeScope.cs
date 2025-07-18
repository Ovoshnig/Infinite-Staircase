using UnityEngine;
using UnityEngine.Audio;
using VContainer;
using VContainer.Unity;

public class ProjectLifetimeScope : LifetimeScope
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private MusicPlayerView _musicPlayerView;

    protected override void Configure(IContainerBuilder builder)
    {
        new DataSavingInstallers().Install(builder);

        new KeyBindingInstaller().Install(builder);

        new SceneInstaller().Install(builder);

        new GamePauseInstaller().Install(builder);

        new InputActionsInstaller().Install(builder);

        new GameSettingsInstaller(_gameSettings).Install(builder);

        new ScreenInstaller().Install(builder);

        new MenuInstaller().Install(builder);

        new AudioMusicInstaller(_musicPlayerView).Install(builder);
        new AudioMusicSceneInstaller().Install(builder);

        new AudioTuningInstaller(_audioMixerGroup).Install(builder);
        new AudioTuningGamePauseInstaller().Install(builder);
        new AudioTuningMenuInstaller().Install(builder);

        new InventoryInstaller().Install(builder);
    }
}
