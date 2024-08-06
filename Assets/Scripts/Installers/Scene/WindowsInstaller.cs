using Zenject;

public class WindowsInstaller : MonoInstaller
{
    public override void InstallBindings() => Container.Bind<WindowTracker>().FromNew().AsSingle();
}
