using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MusicPlayerInstaller : LifetimeScope
{
    [SerializeField] private MusicPlayer _musicPlayer;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(_musicPlayer, 
            Lifetime.Singleton).DontDestroyOnLoad();
    }
}
