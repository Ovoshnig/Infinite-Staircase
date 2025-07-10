using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerWindowSystemMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<WindowTrackerPlayerInputMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<WindowTrackerPlayerScopeViewMediator>(Lifetime.Singleton);

        builder.Register(resolver =>
        {
            CharacterController characterController = resolver.Resolve<CharacterController>();
            return characterController.GetComponentInChildren<PlayerScopeView>();
        }, Lifetime.Singleton);
    }
}
