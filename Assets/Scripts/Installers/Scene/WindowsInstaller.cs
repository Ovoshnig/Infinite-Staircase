using Zenject;

public class WindowsInstaller : MonoInstaller
{
    public override void InstallBindings() => 
        Container.BindInterfacesAndSelfTo<WindowTracker>().FromNew().AsSingle();
}
