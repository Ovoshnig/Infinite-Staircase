using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerAnimationsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register(resolver =>
        {
            CharacterController characterController = resolver.Resolve<CharacterController>();
            return characterController.GetComponentInChildren<PlayerAnimatorView>();
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint<PlayerStateAnimatorViewMediator>(Lifetime.Singleton);
    }
}
