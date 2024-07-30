using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<DraggedItemHolder>().FromNew().AsSingle();
    }
}