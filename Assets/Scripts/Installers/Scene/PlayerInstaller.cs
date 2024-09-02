using System;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _firstPersonCameraPrefab;
    [SerializeField] private GameObject _thirdPersonCameraPrefab;
    [SerializeField] private Transform _spawnPoint;

    public override void InstallBindings()
    {
        Container
            .BindInterfacesAndSelfTo<PlayerInputHandler>()
            .FromNew()
            .AsSingle();

        Container
            .Bind(new Type[] 
            {
                typeof(PlayerState),
                typeof(CameraSwitch), 
                typeof(SkinnedMeshRenderer) 
            })
            .WithId(BindConstants.PlayerId)
            .FromComponentsInNewPrefab(_playerPrefab)
            .UnderTransform(_spawnPoint)
            .AsSingle()
            .NonLazy();

        Container
            .Bind<FirstPersonLook>()
            .WithId(BindConstants.FirstPersonCameraId)
            .FromComponentInNewPrefab(_firstPersonCameraPrefab)
            .AsSingle();

        Container
            .Bind<ThirdPersonLook>()
            .WithId(BindConstants.ThirdPersonCameraId)
            .FromComponentInNewPrefab(_thirdPersonCameraPrefab)
            .AsSingle();
    }
}
