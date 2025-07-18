using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerWindowLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register(resolver =>
        {
            CharacterController characterController = resolver.Resolve<CharacterController>();
            return characterController.GetComponentInChildren<PlayerScopeView>();
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint<CameraSwitchScopeViewMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<PlayerActionsWindowTrackerMediator>(Lifetime.Singleton);
        builder.RegisterEntryPoint<WindowTrackerPlayerScopeViewMediator>(Lifetime.Singleton);
    }
}
