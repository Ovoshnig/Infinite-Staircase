using Zenject;

public class StorageInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SaveStorage>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<SettingsStorage>().FromNew().AsSingle();
    }
}
