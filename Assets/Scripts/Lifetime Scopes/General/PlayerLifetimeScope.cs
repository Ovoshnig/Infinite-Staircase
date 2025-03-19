using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerLifetimeScope : LifetimeScope
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private FirstPersonLook _firstPersonLook;
    [SerializeField] private ThirdPersonLook _thirdPersonLook;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<PlayerInputHandler>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<PlayerState>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<CameraSwitch>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<PlayerVerticalMover>(Lifetime.Singleton).AsSelf();

        builder.Register<PlayerHorizontalMover>(Lifetime.Singleton);

        builder.RegisterComponentInNewPrefab(_characterController, Lifetime.Singleton)
            .UnderTransform(_spawnPoint);

        builder.RegisterComponentInNewPrefab(_firstPersonLook, Lifetime.Singleton);
        builder.RegisterComponentInNewPrefab(_thirdPersonLook, Lifetime.Singleton);
    }

    private void Start()
    {
        GameObject player = Container.Resolve<CharacterController>().gameObject;
        GameObject firstCamera = Container.Resolve<FirstPersonLook>().gameObject;
        GameObject thirdCamera = Container.Resolve<ThirdPersonLook>().gameObject;

        Container.InjectGameObject(player);
        Container.InjectGameObject(firstCamera);
        Container.InjectGameObject(thirdCamera);
    }
}
