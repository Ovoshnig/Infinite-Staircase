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

        builder.Register<PlayerSoundPlayer>(Lifetime.Singleton);
        builder.RegisterEntryPoint<PlayerSoundPlayerMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<PlayerStateSoundPlayerViewMediator>(Lifetime.Singleton);
    }
}
