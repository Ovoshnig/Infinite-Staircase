using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _thirdPersonCameraPrefab;
    [SerializeField] private Transform _spawnPoint;

    public override void InstallBindings()
    {
        Container
            .BindInterfacesAndSelfTo<PlayerInputHandler>()
            .FromNew()
            .AsSingle();

        Container
            .BindInterfacesAndSelfTo<PlayerState>()
            .FromNew()
            .AsSingle();

        Container
            .Bind(new System.Type[] 
            { 
                typeof(CharacterController), 
                typeof(CameraSwitch), 
                typeof(SkinnedMeshRenderer) 
            })
            .WithId(BindConstants.PlayerId)
            .FromComponentsInNewPrefab(_playerPrefab)
            .UnderTransform(_spawnPoint)
            .AsSingle()
            .NonLazy();

        Container
            .Bind<CinemachineCamera>()
            .WithId(BindConstants.ThirdPersonCameraId)
            .FromComponentInNewPrefab(_thirdPersonCameraPrefab)
            .AsSingle();
    }
}
