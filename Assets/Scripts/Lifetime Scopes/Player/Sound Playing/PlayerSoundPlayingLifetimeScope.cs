using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerSoundPlayingLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register(resolver =>
        {
            CharacterController characterController = resolver.Resolve<CharacterController>();
            return characterController.GetComponentInChildren<PlayerSoundPlayerView>();
        }, Lifetime.Singleton);

        builder.Register<PlayerSoundLoader>(Lifetime.Singleton);
        builder.RegisterEntryPoint<PlayerSoundLoaderSoundPlayerViewMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<PlayerStateSoundPlayerViewMediator>(Lifetime.Singleton);
    }
}
