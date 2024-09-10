using Zenject;

public class WindowTrackerInstaller : MonoInstaller
{
    public override void InstallBindings() => 
        Container.BindInterfacesAndSelfTo<WindowTracker>().FromNew().AsSingle();
}
