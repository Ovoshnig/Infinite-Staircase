using Zenject;

public class WindowsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<WindowInputHandler>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<WindowTracker>().FromNew().AsSingle();
    }
}
