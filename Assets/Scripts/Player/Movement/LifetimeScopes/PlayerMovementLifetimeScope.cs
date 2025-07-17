using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerMovementLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register(resolver =>
        {
            CharacterController characterController = resolver.Resolve<CharacterController>();
            return characterController.GetComponentInChildren<PlayerMoverView>();
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint<PlayerMover>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<PlayerMoverMediator>(Lifetime.Singleton).AsSelf();
    }
}
