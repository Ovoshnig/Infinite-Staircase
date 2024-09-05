using Zenject;

public class WindowInputHandlerInstaller : MonoInstaller
{
    public override void InstallBindings() => 
        Container.BindInterfacesAndSelfTo<WindowInputHandler>().FromNew().AsSingle();
}
