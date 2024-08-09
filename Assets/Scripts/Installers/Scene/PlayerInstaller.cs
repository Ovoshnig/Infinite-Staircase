using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _spawnPoint;

    public override void InstallBindings() => Container
        .BindInterfacesAndSelfTo<PlayerInputHandler>()
        .FromNew()
        .AsSingle();

    public override void Start() => Container.InstantiatePrefab(_playerPrefab, _spawnPoint);
}
