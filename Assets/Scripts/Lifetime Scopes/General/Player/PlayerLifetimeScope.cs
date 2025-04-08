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
            return characterController.GetComponentInChildren<FirstCameraPriorityChanger>();
        }, Lifetime.Singleton);

        builder.Register(resolver =>
        {
            CharacterController characterController = resolver.Resolve<CharacterController>();
            return characterController.GetComponentInChildren<ThirdCameraPriorityChanger>();
        }, Lifetime.Singleton);
    }

    private void Start()
    {
        GameObject player = Container.Resolve<CharacterController>().gameObject;
        Container.InjectGameObject(player);
    }
}
