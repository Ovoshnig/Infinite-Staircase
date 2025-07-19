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
    }
}
