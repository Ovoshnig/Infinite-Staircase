using UnityEngine;
using VContainer;
using VContainer.Unity;

public class WindowsPlayerMediatorsLifetimeScope : LifetimeScope
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
