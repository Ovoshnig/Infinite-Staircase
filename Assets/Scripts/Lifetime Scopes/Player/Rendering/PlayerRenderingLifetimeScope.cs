using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerRenderingLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register(resolver =>
        {
            CharacterController characterController = resolver.Resolve<CharacterController>();
            return characterController.GetComponentInChildren<SkinnedMeshRendererView>();
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint<CameraSwitchSkinnedMeshViewMediator>(Lifetime.Singleton);
    }
}
