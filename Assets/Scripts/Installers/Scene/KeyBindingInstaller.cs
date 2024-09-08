using UnityEngine;
using Zenject;

public class KeyBindingInstaller : MonoInstaller
{
    [SerializeField] private ButtonPanelCloser _buttonPanelCloser;

    public override void InstallBindings()
    {
        Container
            .BindInterfacesAndSelfTo<KeyBindingsSaver>()
            .FromNew()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<ButtonPanelCloser>()
            .WithId(ZenjectIdConstants.BindingDoneButtonId)
            .FromInstance(_buttonPanelCloser)
            .AsSingle();
    }
}