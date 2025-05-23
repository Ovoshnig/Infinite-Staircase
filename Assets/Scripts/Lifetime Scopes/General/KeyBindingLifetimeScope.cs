using UnityEngine;
using VContainer;
using VContainer.Unity;

public class KeyBindingLifetimeScope : LifetimeScope
{
    [SerializeField] private ButtonPanelCloser _buttonPanelCloser;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<KeyBindingsTracker>(Lifetime.Singleton).AsSelf();
        builder.RegisterInstance(_buttonPanelCloser);
    }
}
