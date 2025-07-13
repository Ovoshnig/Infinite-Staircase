using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerLifetimeScope : LifetimeScope
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _spawnPoint;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<PlayerInputHandler>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<PlayerState>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<CameraSwitch>(Lifetime.Singleton).AsSelf();

        builder.RegisterComponentInNewPrefab(_characterController, Lifetime.Singleton)
            .UnderTransform(_spawnPoint);

        builder.Register(resolver =>
        {
            CharacterController characterController = resolver.Resolve<CharacterController>();
            return characterController.GetComponentInChildren<PlayerMoverView>();
        }, Lifetime.Singleton);
        builder.RegisterEntryPoint<PlayerMover>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<PlayerMoverMediator>(Lifetime.Singleton).AsSelf();

        builder.Register(resolver =>
        {
            CharacterController characterController = resolver.Resolve<CharacterController>();
            return characterController.GetComponentInChildren<FirstCameraPriorityView>();
        }, Lifetime.Singleton);

        builder.Register(resolver =>
        {
            CharacterController characterController = resolver.Resolve<CharacterController>();
            return characterController.GetComponentInChildren<ThirdCameraPriorityView>();
        }, Lifetime.Singleton);

        builder.Register(resolver =>
        {
            CharacterController characterController = resolver.Resolve<CharacterController>();
            return characterController.GetComponentInChildren<SkinnedMeshRendererView>();
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint<CameraSwitchSkinnedMeshViewMediator>(Lifetime.Singleton);
    }

    private void Start()
    {
        GameObject player = Container.Resolve<CharacterController>().gameObject;
        Container.InjectGameObject(player);
    }
}
