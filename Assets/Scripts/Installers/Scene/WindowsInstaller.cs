using Zenject;

public class WindowsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<WindowInputHandler>().FromNew().AsSingle();
        Container.Bind<WindowTracker>().FromNew().AsSingle();
    }
}
