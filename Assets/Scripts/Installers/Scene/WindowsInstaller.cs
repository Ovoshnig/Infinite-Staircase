using UnityEngine;
using Zenject;

public class WindowsInstaller : MonoInstaller
{
    [SerializeField] private GameObject _playerScopeCanvas;

    public override void InstallBindings()
    {
        GameObject playerScopeCanvas = Instantiate(_playerScopeCanvas);
        Container.Bind<GameObject>()
            .WithId(BindingConstants.PlayerScopeId)
            .FromInstance(playerScopeCanvas)
            .AsTransient();

        Container.BindInterfacesAndSelfTo<WindowTracker>().FromNew().AsSingle();
    }
}
